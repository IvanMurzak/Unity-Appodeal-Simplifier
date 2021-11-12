using System;
using AppodealAds.Unity.Api;
using ConsentManager;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AppodealSimplifier
{
	public static partial class AppodealSimplifier
	{	
		private static		int							initializationFailsCount			= 0;
		private static		PlayerPrefsExDateTime		consentDate							= new PlayerPrefsExDateTime("Appodeal-Conset-Request-Date", DateTime.MinValue);
		private	static		PlayerPrefsExBool			consent								= new PlayerPrefsExBool("Appodeal-Consent", false);
		private static		bool						consentAskedInCurrentSession		= false;

		public	static		bool						IsInitilized						{ get; private set; } = false;
		public	static		bool						IsCached(int adType)				=> IsInitilized && Appodeal.isLoaded(adType);
		public	static		AppodealSimplifierConfig	Config								=> AppodealSimplifierConfigInitializer.settings;

		private	static async UniTask ForceInitialize()
		{
			if (IsInitilized)
			{
				if (Config.debug) Debug.Log("Appodeal already initialized, ignoring second initialization");
				return;
			}

			if (Config.debug) Debug.Log($"Appodeal initialization");
			try
			{
																	Appodeal.setLogLevel(Config.debug ? Config.appodealLogLevel : Appodeal.LogLevel.None);
																	Appodeal.setTesting(Config.appodealTesting);
				if (Config.setChildDirectedTreatment)				Appodeal.setChildDirectedTreatment(true);

				if (!Config.locationPermissionCheck)				Appodeal.disableLocationPermissionCheck();
				//if (!Config.writeExternalStoragePermissionCheck)	Appodeal.exte disableWriteExternalStoragePermissionCheck();

				if (!Config.A4G			)							Appodeal.disableNetwork(AppodealNetworks.A4G		);
				if (!Config.ADCOLONY	)							Appodeal.disableNetwork(AppodealNetworks.ADCOLONY	);
				if (!Config.ADMOB		)							Appodeal.disableNetwork(AppodealNetworks.ADMOB		);
				if (!Config.AMAZON_ADS	)							Appodeal.disableNetwork(AppodealNetworks.AMAZON_ADS	);
				if (!Config.APPODEAL	)							Appodeal.disableNetwork(AppodealNetworks.APPODEAL	);
				if (!Config.APPLOVIN	)							Appodeal.disableNetwork(AppodealNetworks.APPLOVIN	);
				if (!Config.BIDMACHINE	)							Appodeal.disableNetwork(AppodealNetworks.BIDMACHINE	);
				if (!Config.CHARTBOOST	)							Appodeal.disableNetwork(AppodealNetworks.CHARTBOOST	);
				if (!Config.INMOBI		)							Appodeal.disableNetwork(AppodealNetworks.INMOBI		);
				if (!Config.IRONSOURCE	)							Appodeal.disableNetwork(AppodealNetworks.IRONSOURCE	);
				if (!Config.FLURRY		)							Appodeal.disableNetwork(AppodealNetworks.FLURRY		);
				if (!Config.FYBER		)							Appodeal.disableNetwork(AppodealNetworks.FYBER		);
				if (!Config.FACEBOOK	)							Appodeal.disableNetwork(AppodealNetworks.FACEBOOK	);
				if (!Config.MINTEGRAL	)							Appodeal.disableNetwork(AppodealNetworks.MINTEGRAL	);
				if (!Config.MOPUB		)							Appodeal.disableNetwork(AppodealNetworks.MOPUB		);
				if (!Config.MRAID		)							Appodeal.disableNetwork(AppodealNetworks.MRAID		);
				if (!Config.MY_TARGET	)							Appodeal.disableNetwork(AppodealNetworks.MY_TARGET	);
				if (!Config.NAST		)							Appodeal.disableNetwork(AppodealNetworks.NAST		);
				if (!Config.OGURY		)							Appodeal.disableNetwork(AppodealNetworks.OGURY		);
				if (!Config.SMAATO		)							Appodeal.disableNetwork(AppodealNetworks.SMAATO		);
				if (!Config.STARTAPP	)							Appodeal.disableNetwork(AppodealNetworks.STARTAPP	);
				if (!Config.TAPJOY		)							Appodeal.disableNetwork(AppodealNetworks.TAPJOY		);
				if (!Config.YANDEX		)							Appodeal.disableNetwork(AppodealNetworks.YANDEX		);
				if (!Config.UNITY_ADS	)							Appodeal.disableNetwork(AppodealNetworks.UNITY_ADS	);
				if (!Config.VUNGLE		)							Appodeal.disableNetwork(AppodealNetworks.VUNGLE		);
				if (!Config.VAST		)							Appodeal.disableNetwork(AppodealNetworks.VAST		);

				Appodeal.setAutoCache(Appodeal.REWARDED_VIDEO,		Config.AutoCacheRewardedVideo);
				Appodeal.setAutoCache(Appodeal.INTERSTITIAL,		Config.AutoCacheInterstitial);
				Appodeal.setAutoCache(Appodeal.NON_SKIPPABLE_VIDEO, Config.AutoCacheNonSkippableVideo);
				Appodeal.setAutoCache(Appodeal.BANNER,				Config.AutoCacheBanner);
				Appodeal.setAutoCache(Appodeal.MREC,				Config.AutoCacheMREC);

				int adType = 0;
					adType |= Config.EnableRewardedVideo		? Appodeal.REWARDED_VIDEO : 0;
					adType |= Config.EnableNonSkippableVideo	? Appodeal.NON_SKIPPABLE_VIDEO : 0;
					adType |= Config.EnableInterstitial			? Appodeal.INTERSTITIAL : 0;
					adType |= Config.EnableMREC					? Appodeal.MREC : 0;
					adType |= Config.EnableBanner				? Appodeal.BANNER : 0;

				Appodeal.initialize(Config.AppodealAPIKey, adType, consent.Value);
				IsInitilized = true;
				if (Config.debug) Debug.Log($"Appodeal Initialized");
			}
			catch(Exception e)
			{
				Debug.LogError($"Appodeal is not initialized, exception triggered");
				Debug.LogException(e);
				initializationFailsCount++;
				//if (initializationFailsCount == CountOfFailedInitializationUntilReport)
				//{
				//	Analytics.LogError(e);
				//}

				await UniTask.DelayFrame(1);
				await ForceInitialize();
			}		
		}
		public	static void Initialize()
		{
			if (!IsInitilized)
			{
				ForceInitialize().Forget();
			}
			if (Config.support_GDPR_CCPA && Config.support_GDPR_CCPA_OnInitialize)
			{
				RequestConsentIfNeeded();
			}
		}
		public static void RequestConsentIfNeeded()
		{
			if (Config.debug) Debug.Log($"Appodeal.RequestConsentIfNeeded");

			if (!consent.Value && !IsAskedConsentToday() && !consentAskedInCurrentSession 
				|| Config.support_GDPR_CCPA_Force && !consentAskedInCurrentSession)
			{
				AskConsent();
			}
			else
			{
				if (Config.debug) Debug.Log($"Appodeal.RequestConsentIfNeeded already asked today. Will try tomorrow");
			}
		}
		public static void SetConsent(Consent newConsent)
		{
			if (Config.debug) Debug.Log($"Appodeal.updateConsent = {IsDataCollectingAllowed(newConsent)}");

			consent.Value = IsDataCollectingAllowed(newConsent);
			Appodeal.updateConsent(newConsent);
		}
		public static bool IsAskedConsentToday()
		{
			var date	= consentDate.Value;
			var nowDate	= DateTime.Now;

			return date.DayOfYear == nowDate.DayOfYear && date.Year == nowDate.Year;
		}
	}
}