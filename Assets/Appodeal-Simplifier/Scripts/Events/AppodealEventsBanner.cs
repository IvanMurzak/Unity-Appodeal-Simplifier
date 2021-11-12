using AppodealAds.Unity.Common;
using UniRx;
using System;
using UnityEngine;
using AppodealAds.Unity.Api;

namespace AppodealSimplifier
{
	public class AppodealEventsBanner : IBannerAdListener
	{
		private static AppodealEventsBanner _instance;
		public	static AppodealEventsBanner Instance
		{
			get
			{
				if (_instance == null)
				{
					Appodeal.setBannerCallbacks(_instance = new AppodealEventsBanner());
					if (AppodealSimplifier.Config.debug) Debug.Log("Banner Subscribed on Appodeal events");

					_instance.OnBannerLoaded		.Where(x => AppodealSimplifier.Config.debug).Subscribe(data	=> Debug.Log($"Banner.OnBannerLoaded cached={data.precache}, height={data.height}"));
					_instance.OnBannerFailedToLoad	.Where(x => AppodealSimplifier.Config.debug).Subscribe(x	=> Debug.Log($"Banner.OnBannerFailedToLoad"));
					_instance.OnBannerShown			.Where(x => AppodealSimplifier.Config.debug).Subscribe(x	=> Debug.Log($"Banner.OnBannerShown"));
					_instance.OnBannerFinished		.Where(x => AppodealSimplifier.Config.debug).Subscribe(x	=> Debug.Log($"Banner.OnBannerFinished"));
					_instance.OnBannerExpired		.Where(x => AppodealSimplifier.Config.debug).Subscribe(x	=> Debug.Log($"Banner.OnBannerExpired"));
				}
				return _instance;
			}
		}

		Subject<(int height, bool precache)>				_onBannerLoaded					= new Subject<(int height, bool precache)>();
		Subject<Unit>										_onBannerFailedToLoad			= new Subject<Unit>();
		Subject<Unit>										_onBannerShown					= new Subject<Unit>();
		Subject<Unit>										_onBannerFinished				= new Subject<Unit>();
		Subject<Unit>										_onBannerExpired				= new Subject<Unit>();


		public IObservable<(int height, bool precache)>		OnBannerLoaded					=> _onBannerLoaded;
		public IObservable<Unit>							OnBannerFailedToLoad			=> _onBannerFailedToLoad;
		public IObservable<Unit>							OnBannerShown					=> _onBannerShown;
		public IObservable<Unit>							OnBannerFinished				=> _onBannerFinished;
		public IObservable<Unit>							OnBannerExpired					=> _onBannerExpired;


		void IBannerAdListener.onBannerLoaded				(int height, bool precache)		=> _onBannerLoaded			.OnNext((height, precache));
		void IBannerAdListener.onBannerFailedToLoad			()								=> _onBannerFailedToLoad	.OnNext(Unit.Default);
		void IBannerAdListener.onBannerShown				()								=> _onBannerShown			.OnNext(Unit.Default);
		void IBannerAdListener.onBannerClicked				()								=> _onBannerFinished		.OnNext(Unit.Default);
		void IBannerAdListener.onBannerExpired				()								=> _onBannerExpired			.OnNext(Unit.Default);
	}
}