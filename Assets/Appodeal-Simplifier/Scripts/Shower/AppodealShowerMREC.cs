using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UniRx;
using System;
using AppodealAds.Unity.Api;

namespace AppodealSimplifier
{
	public class AppodealShowerMREC : AppodealShowerBase
	{
		protected			Subject<Unit>		onAppodealClosed	= new Subject<Unit>();

		protected override	int					AdType				=> Appodeal.MREC;
		protected override	IObservable<bool>	OnLoadedAds			=> AppodealEventsMREC.Instance.OnMrecLoaded;
		protected override	IObservable<Unit>	OnClosedAds			=> onAppodealClosed;

		protected override void Awake()
		{
			base.Awake();
		}
		protected override void AppodealHide()
		{
			base.AppodealHide();
			onAppodealClosed.OnNext(Unit.Default);
		}
	}
}