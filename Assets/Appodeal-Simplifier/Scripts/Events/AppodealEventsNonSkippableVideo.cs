using AppodealAds.Unity.Common;
using UniRx;
using System;
using UnityEngine;
using AppodealAds.Unity.Api;

namespace AppodealSimplifier
{
	public class AppodealEventsNonSkippableVideo : INonSkippableVideoAdListener
	{
		private static AppodealEventsNonSkippableVideo _instance;
		public	static AppodealEventsNonSkippableVideo Instance
		{
			get
			{
				if (_instance == null)
				{
					Appodeal.setNonSkippableVideoCallbacks(_instance = new AppodealEventsNonSkippableVideo());
					if (AppodealSimplifier.Config.debug) Debug.Log("NonSkippableVideo Subscribed on Appodeal events");

					_instance.OnNonSkippableVideoLoaded			.Where(x => AppodealSimplifier.Config.debug).Subscribe(cached	=> Debug.Log($"NonSkippableVideo.OnNonSkippableVideoLoaded cached={cached}"));
					_instance.OnNonSkippableVideoFailedToLoad	.Where(x => AppodealSimplifier.Config.debug).Subscribe(x		=> Debug.Log($"NonSkippableVideo.OnNonSkippableVideoFailedToLoad"));
					_instance.OnNonSkippableVideoShowFailed		.Where(x => AppodealSimplifier.Config.debug).Subscribe(x		=> Debug.Log($"NonSkippableVideo.OnNonSkippableVideoShowFailed"));
					_instance.OnNonSkippableVideoShown			.Where(x => AppodealSimplifier.Config.debug).Subscribe(x		=> Debug.Log($"NonSkippableVideo.OnNonSkippableVideoShown"));
					_instance.OnNonSkippableVideoFinished		.Where(x => AppodealSimplifier.Config.debug).Subscribe(x		=> Debug.Log($"NonSkippableVideo.OnNonSkippableVideoFinished"));
					_instance.OnNonSkippableVideoClosed			.Where(x => AppodealSimplifier.Config.debug).Subscribe(finished => Debug.Log($"NonSkippableVideo.OnNonSkippableVideoClosed finished={finished}"));
					_instance.OnNonSkippableVideoExpired		.Where(x => AppodealSimplifier.Config.debug).Subscribe(x		=> Debug.Log($"NonSkippableVideo.OnNonSkippableVideoExpired"));
				}
				return _instance;
			}
		}

		Subject<bool>					_onNonSkippableVideoLoaded			= new Subject<bool>();
		Subject<Unit>					_onNonSkippableVideoFailedToLoad	= new Subject<Unit>();
		Subject<Unit>                   _onNonSkippableVideoShowFailed      = new Subject<Unit>();
		Subject<Unit>					_onNonSkippableVideoShown			= new Subject<Unit>();
		Subject<Unit>					_onNonSkippableVideoFinished		= new Subject<Unit>();
		Subject<bool>					_onNonSkippableVideoClosed			= new Subject<bool>();
		Subject<Unit>					_onNonSkippableVideoExpired			= new Subject<Unit>();


		public IObservable<bool>		OnNonSkippableVideoLoaded			=> _onNonSkippableVideoLoaded;
		public IObservable<Unit>		OnNonSkippableVideoFailedToLoad		=> _onNonSkippableVideoFailedToLoad;
		public IObservable<Unit>		OnNonSkippableVideoShowFailed		=> _onNonSkippableVideoShowFailed;
		public IObservable<Unit>		OnNonSkippableVideoShown			=> _onNonSkippableVideoShown;
		public IObservable<Unit>		OnNonSkippableVideoFinished			=> _onNonSkippableVideoFinished;
		public IObservable<bool>		OnNonSkippableVideoClosed			=> _onNonSkippableVideoClosed;
		public IObservable<Unit>		OnNonSkippableVideoExpired			=> _onNonSkippableVideoExpired;


		void INonSkippableVideoAdListener.onNonSkippableVideoLoaded			(bool precache)					=> _onNonSkippableVideoLoaded			.OnNext(precache);
		void INonSkippableVideoAdListener.onNonSkippableVideoFailedToLoad	()								=> _onNonSkippableVideoFailedToLoad		.OnNext(Unit.Default);
		void INonSkippableVideoAdListener.onNonSkippableVideoShowFailed		()								=> _onNonSkippableVideoShowFailed.OnNext(Unit.Default);
		void INonSkippableVideoAdListener.onNonSkippableVideoShown			()								=> _onNonSkippableVideoShown			.OnNext(Unit.Default);
		void INonSkippableVideoAdListener.onNonSkippableVideoFinished		()								=> _onNonSkippableVideoFinished			.OnNext(Unit.Default);
		void INonSkippableVideoAdListener.onNonSkippableVideoClosed			(bool finished)					=> _onNonSkippableVideoClosed			.OnNext(finished);
		void INonSkippableVideoAdListener.onNonSkippableVideoExpired		()								=> _onNonSkippableVideoExpired			.OnNext(Unit.Default);
	}
}