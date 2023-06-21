using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;

using Unity.Services.Analytics;
using Unity.Services.Core;

namespace Assembly.IBX.Analytics
{
    public static class LegalCompliance
    {
        public static bool OptInConsentRequired { get; private set; } = false;
        public static string ConsentIdentifier { get; private set; } = string.Empty;

        /// <summary>
        /// Initialize Analytics Before Awake()
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static async void InitializeAnalytics()
        {
            Debug.Log("Before Scene Load");

            //Wait one second before starting up analytics. This should give time for unity services to initialize
            await Task.Delay(1000);

            //If Unity Services could not be initialized in time, try again...
            if(UnityServices.State == ServicesInitializationState.Uninitialized)
            {
                Debug.LogWarning("Unity Services were not properly initialized, initializing...");
                await UnityServices.InitializeAsync();
            }

            await GetConsents();

            Debug.Log("Analytics Initialized!");
        }

        /// <summary>
        /// Get information about what type of legal consents to adhere to
        /// </summary>
        /// <returns>Awaitable</returns>
        private static async Task GetConsents()
        {
            try
            {
                List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();

                if (consentIdentifiers.Count > 0)
                {
                    if (consentIdentifiers[0].ToUpper().Equals("PIPL") || consentIdentifiers[0].ToUpper() == "PIPL")
                    {
                        OptInConsentRequired = true;
                        ConsentIdentifier = consentIdentifiers[0];
                    }
                }
            }
            catch (ConsentCheckException ex)
            {
                Debug.LogError("Could not check for required analytics consent information due to the following exception.");
                Debug.LogException(ex);
            }
        }

        /// <summary>
        /// GDPR and CCPA Opt Out
        /// </summary>
        public static void GDRPandCCPAAnalyticsOptOut()
        {
            try
            {
                AnalyticsService.Instance.OptOut();
            }
            catch (ConsentCheckException ex)
            {
                Debug.LogError("Could not opt-out of analytics due tot the following exception.");
                Debug.LogException(ex);
            }
        }

        /// <summary>
        /// PIPL Opt In
        /// </summary>
        /// <param name="consentProvided">Did the user consent to both use and export?</param>
        public static void ChinaPIPLOptInAnalyticsConsent(bool consentProvided)
        {
            if (OptInConsentRequired)
            {
                try
                {
                    AnalyticsService.Instance.ProvideOptInConsent(ConsentIdentifier, consentProvided);
                }
                catch (ConsentCheckException ex)
                {
                    Debug.LogError("Could not opt in to analytics due to the following exception.");
                    Debug.LogException(ex);
                }
            }
        }

        /// <summary>
        /// Open the analytics privacy url in the browser
        /// </summary>
        public static string AnalyticsPrivacyURL()
        {
            Application.OpenURL(AnalyticsService.Instance.PrivacyUrl);
            return AnalyticsService.Instance.PrivacyUrl;
        }
    }
}
