using ConsentManager;
using ConsentManager.Common;
using ConsentManager.Platforms;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace AppodealSimplifier
{
	public static partial class AppodealSimplifier
	{
		public static void AskConsent()
		{
			if (Config.debug) Debug.Log($"Appodeal AskConsent");
			
			consentAskedInCurrentSession = true;

			var consentManager = ConsentManagerClientFactory.GetConsentManager();
				consentManager.requestConsentInfoUpdate(Config.AppodealAPIKey, new ConsentManagerListener(consent =>
				{
					SetConsent(consent);

					var consentShouldShow = consentManager.shouldShowConsentDialog();
					if (consentShouldShow == Consent.ShouldShow.TRUE || Config.support_GDPR_CCPA_Force)
					{
						ConsentForm consentForm = null;
						Func<ConsentForm> getConsentForm = () => consentForm;
						consentForm = new ConsentForm.Builder().withListener(new ConsentFormListener(getConsentForm)).build();
						consentForm?.load();
					}
				}));
		}
		private static bool IsDataCollectingAllowed(Consent consent)
		{
			if (Config.debug) Debug.Log($"Appodeal IsDataCollectingAllowed {consent}");

			if (consent.getZone() == Consent.Zone.NONE) return true;
			if (consent.getStatus() == Consent.Status.PERSONALIZED) return true;
			if (consent.getAuthorizationStatus() == Consent.AuthorizationStatus.AUTHORIZED) return true;

			return false;
		}
		public class ConsentManagerListener : IConsentInfoUpdateListener
		{
			private Action<Consent> onInfoUpdate;

			public ConsentManagerListener(Action<Consent> onInfoUpdate)
			{
				this.onInfoUpdate = onInfoUpdate;
			}
			void IConsentInfoUpdateListener.onConsentInfoUpdated(Consent consent)
			{
				if (Config.debug) Debug.Log($"Appodeal ConsentManagerListener.onConsentInfoUpdated, value = {IsDataCollectingAllowed(consent)}");
				UniTask.Post(() => onInfoUpdate.Invoke(consent));
			}
			void IConsentInfoUpdateListener.onFailedToUpdateConsentInfo(ConsentManagerException error)
			{
				if (Config.debug) Debug.LogError($"Appodeal ConsentManagerListener.onFailedToUpdateConsentInfo, reason={error.getReason()}");
			}
		}
		public class ConsentFormListener : IConsentFormListener
		{
			private Func<ConsentForm> consentForm;

			public ConsentFormListener(Func<ConsentForm> consentForm)
			{
				this.consentForm = consentForm;
			}

			void IConsentFormListener.onConsentFormClosed(Consent consent)
			{
				if (Config.debug) Debug.Log($"Appodeal ConsentFormListener.onConsentFormClosed, value = {IsDataCollectingAllowed(consent)}");
				UniTask.Post(() => SetConsent(consent));
			}
			void IConsentFormListener.onConsentFormError(ConsentManagerException consentManagerException)
			{
				if (Config.debug) Debug.LogError($"Appodeal ConsentFormListener.onConsentFormError, reason={consentManagerException.getReason()}");
			}
			void IConsentFormListener.onConsentFormLoaded()
			{
				if (Config.debug) Debug.Log($"Appodeal ConsentFormListener.onConsentFormLoaded");
				consentForm().showAsDialog();
				UniTask.Post(() => consentDate.Value = DateTime.Now);
			}
			void IConsentFormListener.onConsentFormOpened()
			{
				if (Config.debug) Debug.Log($"Appodeal ConsentFormListener.onConsentFormOpened");
			}
		}
	}
}