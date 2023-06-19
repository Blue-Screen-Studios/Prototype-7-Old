using Assembly.IBX.Discord.SDK;
using Assembly.IBX.WebIO;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assembly.IBX.Discord
{
    public class DiscordManager : MonoBehaviour
    {
        private static APITokenSet TOKEN_SET;
        private SDK.Discord discord = null;
        private ActivityManager rpcManager;

        private void Awake()
        {
            Configuration.OnConfigurationDataSet.AddListener(OnConfigurationLoaded);
        }

        private void OnConfigurationLoaded()
        {
            discord = new SDK.Discord(long.Parse(Configuration.discordConfiguration.client_id), (UInt64)CreateFlags.NoRequireDiscord);
        }

        /// <summary>
        /// Sets the user's RPC in Discord.
        /// THIS DOES NOT REQUIRE AUTHORIZATION BECUASE IT IS DONE LOCALLY
        /// </summary>
        /// <param name="rpc">The RPC to set</param>
        public void SetDiscordActivityRPC(Activity rpc)
        {
            rpcManager.UpdateActivity(rpc, (result) =>
            {
                if(result == Result.Ok)
                {
                    Debug.Log("Discord RPC Updated.");
                }
                else
                {
                    Debug.LogError("Failed to update Discord Activity or RPC. This is usually because the user does not have Discord installed on their device.");
                }
            });
        }

        private void OnApplicationQuit()
        {
            //When the application quits we must remember to dispose of the Discord objects to prevent memory leaks
            discord.Dispose();
        }

        private async void Update()
        {
            if(Keyboard.current.f1Key.wasPressedThisFrame)
            {
                await Configuration.GetConfiguration();
                await AuthorizeDiscord();
                await DownloadUserProfileContent();
            }
        }

        /// <summary>
        /// Contains the process for a first time authorization of the Discord OAuth2 Provider
        /// </summary>
        private static async void FirstTimeAuthorizationFlow()
        {
            Debug.Log("Running the first time authorization flow for Discord.");

            //Launch a Discord Authorization prompt in the browser
            OAuth2.LaunchOAuth2Prompt();

            //Extract the authorization code through the authorization prompt on a seperate thread and pause this one
            string authCode = OAuth2.ListenForAuthorizationCode();

            //Initiate an OAuth2 token exchange with Discord through an authorization code grant
            APITokenSet tokens = await OAuth2.TokenExchangeThroughAuthCode(authCode);
            TOKEN_SET = tokens;

            //Serialize and cache this token set with additional data for the first authorization and the latest refresh time
            OAuth2.CreateAndSerializeAPITokenWithLocalTimeData(tokens, true);
        }

        /// <summary>
        /// Starts Discord the OAuth2 Authorization Process
        /// </summary>
        private static async Task AuthorizeDiscord()
        {
            await UGSAuth.InitializeAndAuthenticateCachedUserIfRequired();

            APITokenSetWithLocalTimeData? tokenSetData = OAuth2.DeserializeCachedToken();

            //If we were unable to desrialize a cached token struct...
            if (tokenSetData == null)
            {
                Debug.Log("No cached token set could be found. A new authorization code grant will be requested.");

                //Since we don't have any cached tokens we must go through the first time authorization
                FirstTimeAuthorizationFlow();
            }
            else
            {
                //Here we null-coales the token because we previously checked nullablity and it is not null
                APITokenSetWithLocalTimeData tokenSetDataPopulated = tokenSetData ?? default;

                //Check wether or not a refresh is authorized on this token by the OAuth2 Provider
                bool refreshAuthorized = OAuth2.IsAPITokenRefreshAuthorized(tokenSetDataPopulated);

                //If a token refresh is authorized
                if(refreshAuthorized)
                {
                    Debug.Log("A cached token set was found for Discord. The cached tokens will be refreshed.");

                    //Refresh the tokens
                    APITokenSet tokens = await OAuth2.RefreshExchange(tokenSetDataPopulated);
                    TOKEN_SET = tokens;

                    //Serialize and cache this refreshed token set with additional data fro the latest refresh time
                    OAuth2.CreateAndSerializeAPITokenWithLocalTimeData(tokens, false);
                }
                else
                {
                    Debug.Log("A cached token set was found, however the token set has expired. A new authorization code grant will be requested.");
                    //Since the tokens we have are expired we must go through the first time authorization again
                    FirstTimeAuthorizationFlow();
                }
            }
        }


        /// <summary>
        /// Download and store all relevent profile data for the authorized user
        /// </summary>
        /// <returns>Awaitable asynchronous operation</returns>
        private static async Task DownloadUserProfileContent()
        {
            //Make a web request to the Discord API requesting the data for the current user
            string response = await new IBXWebRequest().Get("https://discord.com/api" + Configuration.discordConfiguration.current_user_api_endpoint, new Dictionary<string, string>
            {
                { "Authorization", $"Bearer {TOKEN_SET.access_token}" }
            });

            //Safely write the user data to disk
            IOSystem.SafeWriteContent("/discord/user.ibx", response, Encoding.UTF8);

            //Serialize the user data into a data structure
            DiscordUserData userData = JsonConvert.DeserializeObject<DiscordUserData>(response);
            
            //Create a CDNResource structs for both the user's avatar and banner
            CDNResource avatarResource = new CDNResource
            {
                resourceType = CDNResource.ResourceType.AVATAR,
                imageFileFormat = IOSystem.ImageFileFormat.PNG,
                clientId = userData.id,
                filename = userData.avatar
            };

            CDNResource bannerResource = new CDNResource
            {
                resourceType = CDNResource.ResourceType.BANNER,
                imageFileFormat = IOSystem.ImageFileFormat.PNG,
                clientId = userData.id,
                filename = userData.banner
            };

            //Build the URI for each resource we request from Discord
            string avatarURI = CDN.BuildResourceURI(avatarResource);
            string bannerURI = CDN.BuildResourceURI(bannerResource);

            //Debug Stuff
            Debug.Log($"Fetching Discord Avatar from {avatarURI}");
            Debug.Log($"Fetching Discord Banner from {bannerURI}");

            //Download the avatar and banner images and convert them to textures
            Texture2D avatar = await CDN.DownloadImageFromURI(avatarURI, 512);
            Texture2D banner = await CDN.DownloadImageFromURI(bannerURI, 512);

            //Safely write these textures to disk for caching purposes
            IOSystem.SafeWriteTexture(avatar, "/discord/avatar", IOSystem.ImageFileFormat.PNG);
            IOSystem.SafeWriteTexture(banner, "/discord/banner", IOSystem.ImageFileFormat.PNG);
        }
    }
}
