using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UniRx;
using Cysharp.Threading.Tasks;
using System;
using AppodealAds.Unity.Api;

namespace AppodealSimplifier
{
	public abstract class AppodealShowerBase : SerializedMonoBehaviour
	{
		string[] Placements() => AppodealSimplifier.Config.placements ?? new string[] { "default" };
#pragma warning disable CS0414 // Remove unused private members
		[ValueDropdown("Placements", IsUniqueList = true)]
        [SerializeField, Required]									string							placement				= "default";
#pragma warning restore CS0414 // Remove unused private members
		[SerializeField]											bool							initializeOnAwake		= true;

		[Button(ButtonSizes.Medium), HorizontalGroup("Buttons")]	void							SimulateShow()			=> TryShow();
		[Button(ButtonSizes.Medium), HorizontalGroup("Buttons")]	void							SimulateHide()			=> Hide();

		[FoldoutGroup("Callbacks")]	public							UnityEvent						onComponentEnabled;
		[FoldoutGroup("Callbacks")]	public							UnityEvent						onNoAds;
		[FoldoutGroup("Callbacks")]	public							UnityEvent						onLoading;
		[FoldoutGroup("Callbacks")]	public							UnityEvent						onLoaded;
		[FoldoutGroup("Callbacks")]	public							UnityEvent						onTryShow;
		[FoldoutGroup("Callbacks")]	public							UnityEvent						onShow;
		[FoldoutGroup("Callbacks")]	public							UnityEvent						onClosed;

									protected						CompositeDisposable				adsShowingDisposables	= new CompositeDisposable();
									protected abstract				int								AdType					{ get; }
									public							bool							IsCached				=> AppodealSimplifier.IsCached(AdType);

									protected						BoolReactiveProperty			AdsVisible				= new BoolReactiveProperty(false);
									public							ReadOnlyReactiveProperty<bool>	IsAdsVisible			=> AdsVisible.ToReadOnlyReactiveProperty();

									protected abstract				IObservable<bool>				OnLoadedAds				{ get; }
									protected abstract				IObservable<Unit>				OnClosedAds				{ get; }

									private							IDisposable						onLoadedDisposable;
									private							IDisposable						onClosedDisposable;

		protected virtual void Awake()
		{
			if (initializeOnAwake) AppodealSimplifier.Initialize();
			if (IsCached) onLoaded?.Invoke();
		}
		protected virtual void OnEnable()
		{
			onComponentEnabled?.Invoke();
		}
		protected virtual bool AppodealShow()
		{
			onShow?.Invoke();

			onClosedDisposable?.Dispose();
			onClosedDisposable = OnClosedAds
				.First		()
				.Subscribe	(_ =>
				{
					AdsVisible.Value = false;
					onClosed?.Invoke();
				})
				.AddTo		(this);

			AdsVisible.Value = Appodeal.show(AdType);
			return AdsVisible.Value;
		}
		protected virtual void AppodealCache() => Appodeal.cache(AdType);
		protected virtual void AppodealHide	()
		{
			Appodeal.hide(AdType);
			AdsVisible.Value = false;
		}

		public void Cache() => TryCache();
		public bool TryCache()
        {
			if (IsCached) return false;
			AppodealCache();
			return true;
        }
		public void Show() => TryShow();
		public virtual bool TryShow()
		{
			AppodealSimplifier.Initialize();

			if (AdsVisible.Value)
			{
				if (AppodealSimplifier.Config.debug) Debug.LogWarning($"Show ads already called for AdType={AdType}, it is still showing");
				return true;
			}
			if (onLoadedDisposable != null)
			{
				if (AppodealSimplifier.Config.debug) Debug.LogWarning($"Ads AdType={AdType}, is still loading");
				return false;
			}
			onTryShow?.Invoke();
			UniTask.Post(async () =>
			{
				await UniTask.DelayFrame(1);
				if (!IsCached)
				{
					if (!AppodealSimplifier.IsInitilized) return;
					if (onLoadedDisposable != null) return;

					onLoading?.Invoke();
					onLoadedDisposable?.Dispose();
					onLoadedDisposable = OnLoadedAds
						.Subscribe(cached =>
						{
							onLoaded?.Invoke();
							onLoadedDisposable?.Dispose();
							onLoadedDisposable = null; 
							AppodealShow();
						}).AddTo(this);
				}
				else
				{
					AppodealShow();
				}
			});
			if (IsCached)
			{
				return AppodealShow();
			}
			else
			{
				if (AppodealSimplifier.Config.debug) Debug.Log($"Ads is not yet cached with AdType={AdType}, calling 'cache' function");
				AppodealCache();
				AdsVisible.Value = false;
				onNoAds?.Invoke();
				return true;
			}
		}
		public virtual void Hide()
		{
			AppodealSimplifier.Initialize();
			AppodealHide();
		}
	}
}