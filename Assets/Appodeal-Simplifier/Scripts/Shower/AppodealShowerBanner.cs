using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UniRx;
using System;
using AppodealAds.Unity.Api;

namespace AppodealSimplifier
{
	public class AppodealShowerBanner : AppodealShowerBase
	{
		protected override	int					AdType		=> Appodeal.BANNER;
		protected override	IObservable<bool>	OnLoadedAds => AppodealEventsBanner.Instance.OnBannerLoaded.Select(x => x.precache);
		protected override	IObservable<Unit>	OnClosedAds => AppodealEventsBanner.Instance.OnBannerFinished;

		protected override void Awake()
		{
			base.Awake();
		}
	}
}