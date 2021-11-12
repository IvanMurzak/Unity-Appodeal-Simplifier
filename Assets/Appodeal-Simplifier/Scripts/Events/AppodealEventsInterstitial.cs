using AppodealAds.Unity.Common;
using UniRx;
using System;
using UnityEngine;
using AppodealAds.Unity.Api;

namespace AppodealSimplifier
{
	public class AppodealEventsInterstitial : IInterstitialAdListener
	{
		private static AppodealEventsInterstitial _instance;
		public	static AppodealEventsInterstitial Instance
		{
			get
			{
				if (_instance == null)
				{
					Appodeal.setInterstitialCallbacks(_instance = new AppodealEventsInterstitial());
					if (AppodealSimplifier.Config.debug) Debug.Log("Interstitial Subscribed on Appodeal events");

					_instance.OnInterstitialLoaded			.Where(x => AppodealSimplifier.Config.debug).Subscribe(cached	=> Debug.Log($"Interstitial.OnInterstitialLoaded cached={cached}"));
					_instance.OnInterstitialFailedToLoad	.Where(x => AppodealSimplifier.Config.debug).Subscribe(x		=> Debug.Log($"Interstitial.OnInterstitialFailedToLoad"));
					_instance.OnInterstitialShown			.Where(x => AppodealSimplifier.Config.debug).Subscribe(x		=> Debug.Log($"Interstitial.OnInterstitialShown"));
					_instance.OnInterstitialShowFailed		.Where(x => AppodealSimplifier.Config.debug).Subscribe(x		=> Debug.Log($"Interstitial.OnInterstitialShowFailed"));
					_instance.OnInterstitialClosed			.Where(x => AppodealSimplifier.Config.debug).Subscribe(x		=> Debug.Log($"Interstitial.OnInterstitialClosed"));
					_instance.OnInterstitialExpired			.Where(x => AppodealSimplifier.Config.debug).Subscribe(x		=> Debug.Log($"Interstitial.OnInterstitialExpired"));
					_instance.OnInterstitialClicked			.Where(x => AppodealSimplifier.Config.debug).Subscribe(x		=> Debug.Log($"Interstitial.OnInterstitialClicked"));
				}
				return _instance;
			}
		}

		Subject<bool>					_onInterstitialLoaded			= new Subject<bool>();
		Subject<Unit>					_onInterstitialFailedToLoad		= new Subject<Unit>();
		Subject<Unit>					_onInterstitialShown			= new Subject<Unit>();
		Subject<Unit>					_onInterstitialShowFailed		= new Subject<Unit>();
		Subject<Unit>					_onInterstitialClosed			= new Subject<Unit>();
		Subject<Unit>					_onInterstitialExpired			= new Subject<Unit>();
		Subject<Unit>					_onInterstitialClicked			= new Subject<Unit>();


		public IObservable<bool>		OnInterstitialLoaded			=> _onInterstitialLoaded;
		public IObservable<Unit>		OnInterstitialFailedToLoad		=> _onInterstitialFailedToLoad;
		public IObservable<Unit>		OnInterstitialShown				=> _onInterstitialShown;
		public IObservable<Unit>		OnInterstitialShowFailed		=> _onInterstitialShowFailed;
		public IObservable<Unit>		OnInterstitialClosed			=> _onInterstitialClosed;
		public IObservable<Unit>		OnInterstitialExpired			=> _onInterstitialExpired;
		public IObservable<Unit>		OnInterstitialClicked			=> _onInterstitialClicked;


		void IInterstitialAdListener.onInterstitialLoaded			(bool precache)	=> _onInterstitialLoaded			.OnNext(precache);
		void IInterstitialAdListener.onInterstitialFailedToLoad		()				=> _onInterstitialFailedToLoad		.OnNext(Unit.Default);
		void IInterstitialAdListener.onInterstitialShown			()				=> _onInterstitialShown				.OnNext(Unit.Default);
		void IInterstitialAdListener.onInterstitialShowFailed		()				=> _onInterstitialShowFailed		.OnNext(Unit.Default);
		void IInterstitialAdListener.onInterstitialClosed			()				=> _onInterstitialClosed			.OnNext(Unit.Default);
		void IInterstitialAdListener.onInterstitialExpired			()				=> _onInterstitialExpired			.OnNext(Unit.Default);
		void IInterstitialAdListener.onInterstitialClicked			()				=> _onInterstitialClicked			.OnNext(Unit.Default);
	}
}