using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UniRx;
using System;
using AppodealAds.Unity.Api;

namespace AppodealSimplifier
{
	public class AppodealShowerNonSkippableVideo : AppodealShowerBase
	{
		protected override	int					AdType		=> Appodeal.NON_SKIPPABLE_VIDEO;
		protected override	IObservable<bool>	OnLoadedAds => AppodealEventsNonSkippableVideo.Instance.OnNonSkippableVideoLoaded;
		protected override	IObservable<Unit>	OnClosedAds => AppodealEventsNonSkippableVideo.Instance.OnNonSkippableVideoClosed.Select(x => Unit.Default);

		protected override void Awake()
		{
			base.Awake();
		}
	}
}