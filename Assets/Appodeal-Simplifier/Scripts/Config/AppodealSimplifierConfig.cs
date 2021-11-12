using AppodealAds.Unity.Api;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AppodealSimplifier
{
#pragma warning disable CA2235 // Mark all non-serializable fields
    public sealed partial class AppodealSimplifierConfig : SerializedScriptableObject
    {
        public const                                                                                                    string              PATH                      = "Assets/Resources/Appodeal-Simplifier-Settings.asset";
        public const                                                                                                    string              PATH_FOR_RESOURCES_LOAD   = "Appodeal-Simplifier-Settings";

        [BoxGroup("0", false), HorizontalGroup("0/1")]
        [TitleGroup("0/1/Appodeal API Key"), SerializeField, LabelText("Android")]                                      string              appodealApiKeyAndroid;
        [TitleGroup("0/1/Appodeal API Key"), SerializeField, LabelText("iOS")]                                          string              appodealApiKeyIOS;

        [BoxGroup("1", false), HorizontalGroup("1/1")]
        [TitleGroup("1/1/Appodeal")]                                                                            public  bool                appodealTesting                         = true;
        [TitleGroup("1/1/Appodeal"), LabelWidth(115)]                                                           public  Appodeal.LogLevel   appodealLogLevel                        = Appodeal.LogLevel.Verbose;
        [TitleGroup("1/1/Debug")]                                                                               public  bool                debug                                   = true;
    
        [HorizontalGroup("1/H")]
        [TitleGroup("1/H/Enable Ad Type"),                              LabelText("Banner")]                    public  bool                EnableBanner                            = true;
        [TitleGroup("1/H/Enable Ad Type"),                              LabelText("MREC")]                      public  bool                EnableMREC                              = true;
        [TitleGroup("1/H/Enable Ad Type"),                              LabelText("Rewarded Video")]            public  bool                EnableRewardedVideo                     = true;
        [TitleGroup("1/H/Enable Ad Type"),                              LabelText("Interstitial")]              public  bool                EnableInterstitial                      = true;
        [TitleGroup("1/H/Enable Ad Type"),                              LabelText("Non Skippable Video")]       public  bool                EnableNonSkippableVideo                 = true;

        [TitleGroup("1/H/Cache"), EnableIf("EnableBanner"),             LabelText("Banner")]                    public  bool                AutoCacheBanner                         = true;
        [TitleGroup("1/H/Cache"), EnableIf("EnableMREC"),               LabelText("MREC")]                      public  bool                AutoCacheMREC                           = true;
        [TitleGroup("1/H/Cache"), EnableIf("EnableRewardedVideo"),      LabelText("Rewarded Video")]            public  bool                AutoCacheRewardedVideo                  = true;
        [TitleGroup("1/H/Cache"), EnableIf("EnableInterstitial"),       LabelText("Interstitial")]              public  bool                AutoCacheInterstitial                   = true;
        [TitleGroup("1/H/Cache"), EnableIf("EnableNonSkippableVideo"),  LabelText("Non Skippable Video")]       public  bool                AutoCacheNonSkippableVideo              = true;

        [BoxGroup("3", false), HorizontalGroup("3/H")]
        [TitleGroup("3/H/Privacy"), LabelText("COPPA - Child Directed Treatment")]                              public  bool                setChildDirectedTreatment               = true;
        [TitleGroup("3/H/Privacy"), LabelText("GDPR and CCPA")]                                                 public  bool                support_GDPR_CCPA                       = true;
        [TitleGroup("3/H/Privacy"), LabelText("GDPR and CCPA ask on Initialize"),  ShowIf("support_GDPR_CCPA")] public  bool                support_GDPR_CCPA_OnInitialize          = true;
        [TitleGroup("3/H/Privacy"), LabelText("GDPR and CCPA ask force"),          ShowIf("support_GDPR_CCPA")] public  bool                support_GDPR_CCPA_Force                 = true;

        [TitleGroup("3/H/Permissions check")]                                                                   public  bool                locationPermissionCheck                 = true;
        //[TitleGroup("3/H/Permissions check")]                                                                   public  bool                writeExternalStoragePermissionCheck     = true;


        [FoldoutGroup("Networks"), HorizontalGroup("Networks/H")]
        [TitleGroup("Networks/H/A-F")]                                                                          public  bool                A4G                                     = true;
        [TitleGroup("Networks/H/A-F")]                                                                          public  bool                ADCOLONY                                = true;
        [TitleGroup("Networks/H/A-F")]                                                                          public  bool                ADMOB                                   = true;
        [TitleGroup("Networks/H/A-F")]                                                                          public  bool                AMAZON_ADS                              = true;
        [TitleGroup("Networks/H/A-F")]                                                                          public  bool                APPODEAL                                = true;
        [TitleGroup("Networks/H/A-F")]                                                                          public  bool                APPLOVIN                                = true;
        [TitleGroup("Networks/H/A-F")]                                                                          public  bool                BIDMACHINE                              = true;
        [TitleGroup("Networks/H/A-F")]                                                                          public  bool                CHARTBOOST                              = true;
        [TitleGroup("Networks/H/A-F")]                                                                          public  bool                INMOBI                                  = true;
        [TitleGroup("Networks/H/A-F")]                                                                          public  bool                IRONSOURCE                              = true;
        [TitleGroup("Networks/H/A-F")]                                                                          public  bool                FLURRY                                  = true;
        [TitleGroup("Networks/H/A-F")]                                                                          public  bool                FYBER                                   = true;
        [TitleGroup("Networks/H/A-F")]                                                                          public  bool                FACEBOOK                                = true;
        [TitleGroup("Networks/H/F-Z")]                                                                          public  bool                MINTEGRAL                               = true;
        [TitleGroup("Networks/H/F-Z")]                                                                          public  bool                MOPUB                                   = true;
        [TitleGroup("Networks/H/F-Z")]                                                                          public  bool                MRAID                                   = true;
        [TitleGroup("Networks/H/F-Z")]                                                                          public  bool                MY_TARGET                               = true;
        [TitleGroup("Networks/H/F-Z")]                                                                          public  bool                NAST                                    = true;
        [TitleGroup("Networks/H/F-Z")]                                                                          public  bool                OGURY                                   = true;
        [TitleGroup("Networks/H/F-Z")]                                                                          public  bool                SMAATO                                  = true;
        [TitleGroup("Networks/H/F-Z")]                                                                          public  bool                STARTAPP                                = true;
        [TitleGroup("Networks/H/F-Z")]                                                                          public  bool                TAPJOY                                  = true; 
        [TitleGroup("Networks/H/F-Z")]                                                                          public  bool                YANDEX                                  = true;
        [TitleGroup("Networks/H/F-Z")]                                                                          public  bool                UNITY_ADS                               = true;
        [TitleGroup("Networks/H/F-Z")]                                                                          public  bool                VUNGLE                                  = true;
        [TitleGroup("Networks/H/F-Z")]                                                                          public  bool                VAST                                    = true;

        [Required, HideReferenceObjectPicker, BoxGroup("Placements", false)]                                    public  string[]            placements                              = new string[] { "default" };

#if UNITY_ANDROID
                                                                                                                public  string			    AppodealAPIKey							=> appodealApiKeyAndroid;
#elif UNITY_IOS
		                                                                                                        public  string			    AppodealAPIKey							=> appodealApiKeyIOS;
#endif
    }
#pragma warning restore CA2235 // Mark all non-serializable fields
}