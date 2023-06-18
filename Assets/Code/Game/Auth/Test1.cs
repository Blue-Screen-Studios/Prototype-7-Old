using Assembly.IBX.Remote;
using System.IO;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEditor;
using UnityEngine;

namespace Assembly.IBX.Auth
{
    public class Test1 : MonoBehaviour
    {
        private async void Start()
        {
            


        }

        public static async void AuthorizeDiscord()
        {
            await UGSAuth.InitializeAndAuthenticateCachedUserIfRequired();

            APITokenSetWithLocalTimeData? tokenSetData = DiscordOauth2.DeserializeCachedToken();

            //If we were unable to desrialize a cached token struct...
            if (tokenSetData == null)
            {
                //Launch a Discord Authorization prompt in the browser
                DiscordOauth2.LaunchOAuth2Prompt();

                //Extract the authorization code through the authorization prompt on a seperate thread and pause this one
                string authCode = DiscordOauth2.ListenForAuthorizationCode();
                
                //Initiate an OAuth2 token exchange with Discord through an authorization code grant
                APITokenSet tokens = await DiscordOauth2.TokenExchangeThroughAuthCode(authCode);

                //Serialize and cache this token set with additional data for the first authorization and the latest refresh time
                DiscordOauth2.CreateAndSerializeAPITokenWithLocalTimeData(tokens, true);                
            }
            else
            {
                //Herw we null-coales the token because we previously checked nullablity and it is not null
                APITokenSetWithLocalTimeData tokenSetDataPopulated = tokenSetData ?? default;
            }
        }
    }
}
