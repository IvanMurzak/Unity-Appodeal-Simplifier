using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UniRx;
using System;
using AppodealAds.Unity.Api;

namespace AppodealSimplifier
{
	public class AppodealShowerRewardedVideo : AppodealShowerBase
	{
									public	AppodealEventsRewardedVideo.Reward				reward;

		[FoldoutGroup("Callbacks")] public	UnityEvent										onRewardedSimple;
		[FoldoutGroup("Callbacks")] public	UnityEvent<AppodealEventsRewardedVideo.Reward>	onRewarded;

		protected override	int					AdType		=> Appodeal.REWARDED_VIDEO;
		protected override	IObservable<bool>	OnLoadedAds => AppodealEventsRewardedVideo.Instance.OnRewardedVideoLoaded;
		protected override	IObservable<Unit>	OnClosedAds => AppodealEventsRewardedVideo.Instance.OnRewardedVideoClosed.Select(x => Unit.Default);

		protected IDisposable rewardedVideoClosedDisposable;

		protected override void Awake()
		{
			base.Awake();

			onShow.AsObservable()
				.Subscribe(x =>
				{
					rewardedVideoClosedDisposable?.Dispose();
					rewardedVideoClosedDisposable = AppodealEventsRewardedVideo.Instance.OnRewardedVideoClosed
						.Where		(finished => finished)
						.Where		(finished => AdsVisible.Value)
						.DelayFrame	(3)
						.Subscribe	(finished =>
						{
							onRewardedSimple?.Invoke();
							onRewarded?.Invoke(reward);
						})
						.AddTo		(this);

					OnClosedAds.First()
						.DelayFrame	(10)
						.Subscribe	(_ => rewardedVideoClosedDisposable.Dispose())
						.AddTo		(this);
				})
				.AddTo(this);
			
			//AppodealEventsRewardedVideo.Instance.OnRewardedVideoClosed
			//	.Where		(x => AdsVisible.Value)
			//	.Subscribe	(x => onRewardedVideoClosed?.Invoke(x))
			//	.AddTo		(this);

			//AppodealEventsRewardedVideo.Instance.OnRewardedVideoExpired
			//	.Where		(x => AdsVisible.Value)
			//	.Subscribe	(x => onRewardedVideoExpired?.Invoke())
			//	.AddTo		(this);

			//AppodealEventsRewardedVideo.Instance.OnRewardedVideoFailedToLoad
			//	.Where		(x => AdsVisible.Value)
			//	.Subscribe	(x => onRewardedVideoFailedToLoad?.Invoke())
			//	.AddTo		(this);

			//AppodealEventsRewardedVideo.Instance.OnRewardedVideoFinished
			//	.Where		(x => AdsVisible.Value)
			//	.Subscribe	(x => onRewardedVideoFinished?.Invoke(x))
			//	.AddTo		(this);

			//AppodealEventsRewardedVideo.Instance.OnRewardedVideoLoaded
			//	.Where		(x => AdsVisible.Value)
			//	.Subscribe	(x => onRewardedVideoLoaded?.Invoke(x))
			//	.AddTo		(this);

			//AppodealEventsRewardedVideo.Instance.OnRewardedVideoShowFailed
			//	.Where		(x => AdsVisible.Value)
			//	.Subscribe	(x => onRewardedVideoShowFailed?.Invoke())
			//	.AddTo		(this);

			//AppodealEventsRewardedVideo.Instance.OnRewardedVideoShown
			//	.Where		(x => AdsVisible.Value)
			//	.Subscribe	(x => onRewardedVideoShown?.Invoke())
			//	.AddTo		(this);
		}
	}
}