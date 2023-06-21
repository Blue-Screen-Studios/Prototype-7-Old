using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.RemoteConfig;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine.Events;

namespace Assembly.IBX.WebIO
{
    public static partial class Configuration
    {
        public static UnityEvent OnConfigurationDataSet = new UnityEvent();

        public static async Task GetAndLoadConfiguration()
        {
            if (Utilities.CheckForInternetConnection())
            {
                if (UnityServices.State == ServicesInitializationState.Uninitialized)
                {
                    Debug.LogWarning("Unity Services are not initialized. Initializing unity services.");
                    await UnityServices.InitializeAsync();
                }

                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    Debug.LogWarning("The client is unauthorized. Attempting anonymous reauthorizaiton.");
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                }

                RemoteConfigService.Instance.FetchCompleted += RecievedConfigs;

                await RemoteConfigService.Instance.FetchConfigsAsync(new DiscordConfiguration(), new DiscordConfiguration());
            }
        }

        private static void RecievedConfigs(ConfigResponse response)
        {
            switch(response.requestOrigin)
            {
                case ConfigOrigin.Cached:
                    Debug.Log("No new configs loaded this session. Using cached values from a previous session.");
                    break;
                case ConfigOrigin.Remote:
                    Debug.Log("New configs were loaded this session.");
                    break;
                default:
                    Debug.Log("No configs were loaded this session and no cache file exists. Implementing default configs.");
                    break;
            }

            OAuth2Port = RemoteConfigService.Instance.appConfig.GetInt("OAuth2 Port");
            discordConfiguration = JsonConvert.DeserializeObject<DiscordConfiguration>(RemoteConfigService.Instance.appConfig.GetJson("Discord Configuration"));

            OnConfigurationDataSet.Invoke();
        }
    }
}
