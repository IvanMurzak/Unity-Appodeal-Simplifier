using AppodealAds.Unity.Common;
using UniRx;
using System;
using UnityEngine;
using AppodealAds.Unity.Api;

namespace AppodealSimplifier
{
	public class AppodealEventsMREC : IMrecAdListener
	{
		private static AppodealEventsMREC _instance;
		public	static AppodealEventsMREC Instance
		{
			get
			{
				if (_instance == null)
				{
					Appodeal.setMrecCallbacks(_instance = new AppodealEventsMREC());
					if (AppodealSimplifier.Config.debug) Debug.Log("MREC Subscribed on Appodeal events");

					_instance.OnMrecLoaded			.Where(x => AppodealSimplifier.Config.debug).Subscribe(cached	=> Debug.Log($"MREC.OnMrecLoaded cached={cached}"));
					_instance.OnMrecFailedToLoad	.Where(x => AppodealSimplifier.Config.debug).Subscribe(x		=> Debug.Log($"MREC.OnMrecFailedToLoad"));
					_instance.OnMrecShown			.Where(x => AppodealSimplifier.Config.debug).Subscribe(x		=> Debug.Log($"MREC.OnMrecShown"));
					_instance.OnMrecClicked			.Where(x => AppodealSimplifier.Config.debug).Subscribe(x		=> Debug.Log($"MREC.OnMrecClicked"));
					_instance.OnMrecExpired			.Where(x => AppodealSimplifier.Config.debug).Subscribe(x		=> Debug.Log($"MREC.OnMrecExpired"));
				}
				return _instance;
			}
		}

		Subject<bool>					_onMrecLoaded			= new Subject<bool>();
		Subject<Unit>					_onMrecFailedToLoad		= new Subject<Unit>();
		Subject<Unit>					_onMrecShown			= new Subject<Unit>();
		Subject<Unit>					_onMrecClicked			= new Subject<Unit>();
		Subject<Unit>					_onMrecExpired			= new Subject<Unit>();


		public IObservable<bool>		OnMrecLoaded			=> _onMrecLoaded;
		public IObservable<Unit>		OnMrecFailedToLoad		=> _onMrecFailedToLoad;
		public IObservable<Unit>		OnMrecShown				=> _onMrecShown;
		public IObservable<Unit>		OnMrecClicked			=> _onMrecClicked;
		public IObservable<Unit>		OnMrecExpired			=> _onMrecExpired;


		void IMrecAdListener.onMrecLoaded			(bool precache)	=> _onMrecLoaded		.OnNext(precache);
		void IMrecAdListener.onMrecFailedToLoad		()				=> _onMrecFailedToLoad	.OnNext(Unit.Default);
		void IMrecAdListener.onMrecShown			()				=> _onMrecShown			.OnNext(Unit.Default);
		void IMrecAdListener.onMrecClicked			()				=> _onMrecClicked		.OnNext(Unit.Default);
		void IMrecAdListener.onMrecExpired			()				=> _onMrecExpired		.OnNext(Unit.Default);
	}
}