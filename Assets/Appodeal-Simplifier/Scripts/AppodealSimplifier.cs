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
		public	static		bool						IsCached(int adType)
		{
			if (IsInitilized)
			{
				if (Config.debug) Debug.Log($"Appodeal.isLoaded({adType})");
				return Appodeal.isLoaded(adType);
			}
			return false;
		}
		public	static		AppodealSimplifierConfig	Config								=> AppodealSimplifierConfigInitializer.Config;

		private	static void ForceInitialize()
		{
			Debug.Log($"AppodealSimplifier ForceInitialize called raw");
			if (Config.debug) Debug.Log($"AppodealSimplifier ForceInitialize called");
			if (IsInitilized)
			{
				if (Config.debug) Debug.Log("Appodeal already initialized, ignoring second initialization");
				return;
			}

			try
			{
				int adType = 0;
					adType |= Config.EnableRewardedVideo		? Appodeal.REWARDED_VIDEO : 0;
					adType |= Config.EnableNonSkippableVideo	? Appodeal.NON_SKIPPABLE_VIDEO : 0;
					adType |= Config.EnableInterstitial			? Appodeal.INTERSTITIAL : 0;
					adType |= Config.EnableMREC					? Appodeal.MREC : 0;
					adType |= Config.EnableBanner				? Appodeal.BANNER : 0;

				if (Config.debug)
				{
					Debug.Log($"Appodeal.initialize(APIKEY={Config.AppodealAPIKey}, adType={adType}, consent={consent.Value}");
					Debug.Log($"Appodeal initialization Appodeal.REWARDED_VIDEO={Config.EnableRewardedVideo}");
					Debug.Log($"Appodeal initialization Appodeal.NON_SKIPPABLE_VIDEO={Config.EnableNonSkippableVideo}");
					Debug.Log($"Appodeal initialization Appodeal.INTERSTITIAL={Config.EnableInterstitial}");
					Debug.Log($"Appodeal initialization Appodeal.MREC={Config.EnableMREC}");
					Debug.Log($"Appodeal initialization Appodeal.BANNER={Config.EnableBanner}");
				}

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

				SetupAutoCache();

				Appodeal.initialize(Config.AppodealAPIKey, adType, consent.Value);
				IsInitilized = true;
				if (Config.debug) Debug.Log($"Appodeal Initialized");
			}
			catch(Exception e)
			{
				Debug.LogError($"Appodeal is not initialized, exception triggered");
				Debug.LogException(e);
				initializationFailsCount++;

				UniTask.Post(async () =>
				{
					await UniTask.DelayFrame(1);
					ForceInitialize();
				});
			}		
		}
		private static void SetupAutoCache()
        {
			if (Config.EnableRewardedVideo)		Appodeal.setAutoCache(Appodeal.REWARDED_VIDEO,		Config.AutoCacheRewardedVideo);
			if (Config.EnableInterstitial)		Appodeal.setAutoCache(Appodeal.INTERSTITIAL,		Config.AutoCacheInterstitial);
			if (Config.EnableNonSkippableVideo) Appodeal.setAutoCache(Appodeal.NON_SKIPPABLE_VIDEO, Config.AutoCacheNonSkippableVideo);
			if (Config.EnableBanner)			Appodeal.setAutoCache(Appodeal.BANNER,				Config.AutoCacheBanner);
			if (Config.EnableMREC)				Appodeal.setAutoCache(Appodeal.MREC,				Config.AutoCacheMREC);
        }
		public	static void Initialize()
		{
			if (!IsInitilized)
			{
				ForceInitialize();
			}
			if (Config.support_GDPR_CCPA && Config.support_GDPR_CCPA_OnInitialize)
			{
				RequestConsentIfNeeded();
			}
		}
		public static void RequestConsentIfNeeded()
		{
			if (Config.debug) Debug.Log($"Appodeal.RequestConsentIfNeeded");

			if (consentAskedInCurrentSession) return;
			if (IsAskedConsentToday() && !Config.support_GDPR_CCPA_Force)
			{
				if (Config.debug) Debug.Log($"Appodeal.RequestConsentIfNeeded already asked today. Will try tomorrow");
				return;
			}
			if (consent.Value)
            {
				if (Config.debug) Debug.Log($"Consent already granted.");
				return;
			}
			AskConsent();
		}
		public static void SetConsent(Consent newConsent)
		{
			if (Config.debug) Debug.Log($"Appodeal.updateConsent = {IsDataCollectingAllowed(newConsent)}");

			consent.Value = IsDataCollectingAllowed(newConsent);
			Appodeal.updateConsent(newConsent);
			SetupAutoCache();
		}
		public static bool IsAskedConsentToday()
		{
			var date	= consentDate.Value;
			var nowDate	= DateTime.Now;

			return date.DayOfYear == nowDate.DayOfYear && date.Year == nowDate.Year;
		}
	}
}