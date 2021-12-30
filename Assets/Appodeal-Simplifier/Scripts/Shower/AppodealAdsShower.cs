using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UniRx;
using Cysharp.Threading.Tasks;
using System;
using AppodealAds.Unity.Api;

namespace AppodealSimplifier
{
	public abstract class AppodealAdsShower : SerializedMonoBehaviour
	{
		string[] Placements() => AppodealSimplifier.Config.placements ?? new string[] { "default" };
		[ValueDropdown("Placements", IsUniqueList = true)]
		[SerializeField, Required]									string							placement				= "default";
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
			AppodealSimplifier.Initialize();

			onShow?.Invoke();

			onClosedDisposable?.Dispose();
			onClosedDisposable = OnClosedAds
				.First		()
				.Subscribe	(_ => onClosed?.Invoke())
				.AddTo		(this);

			if (AppodealSimplifier.Config.debug) Debug.Log($"Appodeal.show({AdType})");
			AdsVisible.Value = Appodeal.show(AdType);
			return AdsVisible.Value;
		}
		protected virtual void AppodealCache()
		{
			if (AppodealSimplifier.IsInitilized)
			{
				if (AppodealSimplifier.Config.debug) Debug.Log($"Appodeal.cache({AdType})");
				Appodeal.cache(AdType);
			}
		}
		protected virtual void AppodealHide	()
		{
			if (AppodealSimplifier.IsInitilized)
			{
				if (AppodealSimplifier.Config.debug) Debug.Log($"Appodeal.hide({AdType})");
				Appodeal.hide(AdType);
				AdsVisible.Value = false;
			}
		}

		public void Show()
		{
			AppodealSimplifier.Initialize();
			TryShow();
		}
		public virtual bool TryShow()
		{
			AppodealSimplifier.Initialize();

			if (AdsVisible.Value)
			{
				Debug.LogWarning($"Show ads already called for AdType={AdType}, it is still showing");
				return true;
			}
			if (onLoadedDisposable != null)
			{
				Debug.LogWarning($"Ads AdType={AdType}, is still loading");
				return false;
			}
			onTryShow?.Invoke();
			
			if (IsCached)
			{
				UniTask.Post(async () =>
				{
					await UniTask.DelayFrame(1);
					AppodealShow();
				});
			}
			else
			{
				Debug.Log($"Ads is not yet cached with AdType={AdType}, calling 'cache' function");
				AppodealCache();
				AdsVisible.Value = false;
				onNoAds?.Invoke();

				UniTask.Post(async () =>
				{
					await UniTask.DelayFrame(1);
					if (!AppodealSimplifier.IsInitilized) return;
					if (onLoadedDisposable != null) return;

					onLoading?.Invoke();
					onLoadedDisposable?.Dispose();
					onLoadedDisposable = OnLoadedAds
						.Subscribe(cached =>
						{
							onLoaded?.Invoke();
							onLoadedDisposable?.Dispose();
							AppodealShow();
						}).AddTo(this);
				});
			}
			return IsCached;
		}
		public virtual void Hide()
		{
			AppodealSimplifier.Initialize();
			AppodealHide();
		}
	}
}