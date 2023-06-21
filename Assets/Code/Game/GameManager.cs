using Assembly.IBX.Discord;
using Assembly.IBX.Discord.SDK;
using Assembly.IBX.WebIO;

using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

namespace Assembly.IBX.Main
{
    public class GameManager : MonoBehaviour
    {
        private static Discord.SDK.Discord discord;

        private async void Awake()
        {
            //Once our game config is loaded execute the OnConfigurationLoaded() method
            Configuration.OnConfigurationDataSet.AddListener(OnConfigurationLoaded);

            //Get the current Game Configuration
            await Configuration.GetAndLoadConfiguration();
        }

        private void OnApplicationQuit()
        {
            DiscordManager.Dispose();
        }

        private static async void OnConfigurationLoaded()
        {
            //Initialize the Discord Game SDK
            DiscordManager.InitializeSDK();

            //Discord Automatic Authorization
            if (File.Exists(Application.streamingAssetsPath + "/discord/auth.ibx"))
            {
                Debug.Log("Discord Auth file found. Authorizing Discord...");

                await DiscordManager.AuthorizeDiscord();
            }
            else
            {
                Debug.Log("No Discord Auth file found. Discord will not be automatically authorized.");
            }
        }
    }
}
