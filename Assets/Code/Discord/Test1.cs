using Assembly.IBX.Remote;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Assembly.IBX.Discord
{
    public class Test1 : MonoBehaviour
    {
        private async void Awake()
        {
            Configuration.OnConfigurationDataSet.AddListener(AuthorizeDiscord);

            await Configuration.GetConfiguration();
        }


        /// <summary>
        /// Contains the process for a first time authorization of the Discord OAuth2 Provider
        /// </summary>
        private static async void FirstTimeAuthorizationFlow()
        {
            Debug.Log("Running the first time authorization flow for Discord.");

            //Launch a Discord Authorization prompt in the browser
            DiscordOauth2.LaunchOAuth2Prompt();

            //Extract the authorization code through the authorization prompt on a seperate thread and pause this one
            string authCode = DiscordOauth2.ListenForAuthorizationCode();

            //Initiate an OAuth2 token exchange with Discord through an authorization code grant
            APITokenSet tokens = await DiscordOauth2.TokenExchangeThroughAuthCode(authCode);

            //Serialize and cache this token set with additional data for the first authorization and the latest refresh time
            DiscordOauth2.CreateAndSerializeAPITokenWithLocalTimeData(tokens, true);
        }

        /// <summary>
        /// Starts Discord the OAuth2 Authorization Process
        /// </summary>
        public static async void AuthorizeDiscord()
        {
            await UGSAuth.InitializeAndAuthenticateCachedUserIfRequired();

            APITokenSetWithLocalTimeData? tokenSetData = DiscordOauth2.DeserializeCachedToken();

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
                    APITokenSet tokens = await DiscordOauth2.RefreshExchange(tokenSetDataPopulated);

                    //Serialize and cache this refreshed token set with additional data fro the latest refresh time
                    DiscordOauth2.CreateAndSerializeAPITokenWithLocalTimeData(tokens, false);
                }
                else
                {
                    Debug.Log("A cached token set was found, however the token set has expired. A new authorization code grant will be requested.");
                    //Since the tokens we have are expired we must go through the first time authorization again
                    FirstTimeAuthorizationFlow();
                }
            }
        }
    }
}
