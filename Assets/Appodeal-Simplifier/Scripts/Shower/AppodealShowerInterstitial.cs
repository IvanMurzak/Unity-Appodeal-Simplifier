using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UniRx;
using System;
using AppodealAds.Unity.Api;

namespace AppodealSimplifier
{
	public class AppodealShowerInterstitial : AppodealShowerBase
	{
		protected override	int					AdType		=> Appodeal.INTERSTITIAL;
		protected override	IObservable<bool>	OnLoadedAds => AppodealEventsInterstitial.Instance.OnInterstitialLoaded;
		protected override	IObservable<Unit>	OnClosedAds => AppodealEventsInterstitial.Instance.OnInterstitialClosed;

		protected override void Awake()
		{
			base.Awake();
		}
	}
}