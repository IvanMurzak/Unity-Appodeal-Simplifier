using AppodealAds.Unity.Common;
using UniRx;
using System;
using UnityEngine;
using AppodealAds.Unity.Api;

namespace AppodealSimplifier
{
	public class AppodealEventsRewardedVideo : IRewardedVideoAdListener
	{
		private static AppodealEventsRewardedVideo _instance;
		public	static AppodealEventsRewardedVideo Instance
		{
			get
			{
				if (_instance == null)
				{
					Appodeal.setRewardedVideoCallbacks(_instance = new AppodealEventsRewardedVideo());
					if (AppodealSimplifier.Config.debug) Debug.Log("RewardedVideo Subscribed on Appodeal events");

					_instance.OnRewardedVideoLoaded			.Where(x => AppodealSimplifier.Config.debug).Subscribe(cached	=> Debug.Log($"RewardedVideo.OnRewardedVideoLoaded cached={cached}"));
					_instance.OnRewardedVideoFailedToLoad	.Where(x => AppodealSimplifier.Config.debug).Subscribe(x		=> Debug.Log($"RewardedVideo.OnRewardedVideoFailedToLoad"));
					_instance.OnRewardedVideoShown			.Where(x => AppodealSimplifier.Config.debug).Subscribe(x		=> Debug.Log($"RewardedVideo.OnRewardedVideoShown"));
					_instance.OnRewardedVideoFinished		.Where(x => AppodealSimplifier.Config.debug).Subscribe(reward	=> Debug.Log($"RewardedVideo.OnRewardedVideoFinished reward({reward.name} {reward.amount})"));
					_instance.OnRewardedVideoClosed			.Where(x => AppodealSimplifier.Config.debug).Subscribe(finished => Debug.Log($"RewardedVideo.OnRewardedVideoClosed finished={finished}"));
					_instance.OnRewardedVideoExpired		.Where(x => AppodealSimplifier.Config.debug).Subscribe(x		=> Debug.Log($"RewardedVideo.OnRewardedVideoExpired"));
					_instance.OnRewardedVideoClicked		.Where(x => AppodealSimplifier.Config.debug).Subscribe(x		=> Debug.Log($"RewardedVideo.OnRewardedVideoClicked"));
					_instance.OnRewardedVideoShowFailed		.Where(x => AppodealSimplifier.Config.debug).Subscribe(x		=> Debug.Log($"RewardedVideo.OnRewardedVideoShowFailed"));
				}
				return _instance;
			}
		}

		Subject<bool>					_onRewardedVideoLoaded			= new Subject<bool>();
		Subject<Unit>					_onRewardedVideoFailedToLoad	= new Subject<Unit>();
		Subject<Unit>					_onRewardedVideoShown			= new Subject<Unit>();
		Subject<Reward>					_onRewardedVideoFinished		= new Subject<Reward>();
		Subject<bool>					_onRewardedVideoClosed			= new Subject<bool>();
		Subject<Unit>					_onRewardedVideoExpired			= new Subject<Unit>();
		Subject<Unit>					_onRewardedVideoClicked			= new Subject<Unit>();
		Subject<Unit>                   _onRewardedVideoShowFailed      = new Subject<Unit>();


		public IObservable<bool>		OnRewardedVideoLoaded			=> _onRewardedVideoLoaded;
		public IObservable<Unit>		OnRewardedVideoFailedToLoad		=> _onRewardedVideoFailedToLoad;
		public IObservable<Unit>		OnRewardedVideoShown			=> _onRewardedVideoShown;
		public IObservable<Reward>		OnRewardedVideoFinished			=> _onRewardedVideoFinished;
		public IObservable<bool>		OnRewardedVideoClosed			=> _onRewardedVideoClosed;
		public IObservable<Unit>		OnRewardedVideoExpired			=> _onRewardedVideoExpired;
		public IObservable<Unit>		OnRewardedVideoClicked			=> _onRewardedVideoClicked;
		public IObservable<Unit>		OnRewardedVideoShowFailed		=> _onRewardedVideoShowFailed;


		void IRewardedVideoAdListener.onRewardedVideoLoaded			(bool precache)					=> _onRewardedVideoLoaded			.OnNext(precache);
		void IRewardedVideoAdListener.onRewardedVideoFailedToLoad	()								=> _onRewardedVideoFailedToLoad		.OnNext(Unit.Default);
		void IRewardedVideoAdListener.onRewardedVideoShown			()								=> _onRewardedVideoShown			.OnNext(Unit.Default);
		void IRewardedVideoAdListener.onRewardedVideoFinished		(double amount, string name)	=> _onRewardedVideoFinished			.OnNext(new Reward() { amount = amount, name = name });
		void IRewardedVideoAdListener.onRewardedVideoClosed			(bool finished)					=> _onRewardedVideoClosed			.OnNext(finished);
		void IRewardedVideoAdListener.onRewardedVideoExpired		()								=> _onRewardedVideoExpired			.OnNext(Unit.Default);
		void IRewardedVideoAdListener.onRewardedVideoClicked		()								=> _onRewardedVideoClicked			.OnNext(Unit.Default);
		void IRewardedVideoAdListener.onRewardedVideoShowFailed		()								=> _onRewardedVideoShowFailed		.OnNext(Unit.Default);

		[Serializable]
		public class Reward
		{
			public double amount;
			public string name;
		}
	}
}