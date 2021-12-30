using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UniRx;
using System;
using AppodealAds.Unity.Api;
using System.Collections.Generic;

namespace AppodealSimplifier
{
	public class AppodealShowerBanner : AppodealShowerBase
	{
		static Dictionary<Side, int> appodealSide = new Dictionary<Side, int>()
		{
			{ Side.Default,				Appodeal.BANNER },
			{ Side.Left,				Appodeal.BANNER_LEFT },
			{ Side.Top,					Appodeal.BANNER_TOP },
			{ Side.Right,				Appodeal.BANNER_RIGHT },
			{ Side.Bottom,				Appodeal.BANNER_BOTTOM },
			{ Side.View,				Appodeal.BANNER_VIEW },
			{ Side.HorizontalLeft,		Appodeal.BANNER_HORIZONTAL_LEFT },
			{ Side.HorizontalRight,		Appodeal.BANNER_HORIZONTAL_RIGHT },
			{ Side.HorizontalCenter,	Appodeal.BANNER_HORIZONTAL_CENTER },
			{ Side.HorizontalSmart,		Appodeal.BANNER_HORIZONTAL_SMART }
		};

		[SerializeField]	Side				side		= Side.Default;
		protected override	int					AdType		=> appodealSide[side];
		protected override	IObservable<bool>	OnLoadedAds => AppodealEventsBanner.Instance.OnBannerLoaded.Select(x => x.precache);
		protected override	IObservable<Unit>	OnClosedAds => AppodealEventsBanner.Instance.OnBannerFinished;

		protected override void Awake()
		{
			base.Awake();
		}

		public enum Side
        {
			Default, Left, Top, Right, Bottom, View,
			HorizontalLeft, HorizontalCenter, HorizontalRight, HorizontalSmart
		}
	}
}