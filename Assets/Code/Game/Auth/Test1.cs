using Assembly.IBX.Remote;
using System.IO;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Assembly.IBX.Auth
{
    public class Test1 : MonoBehaviour
    {
        private async void Start()
        {
            await UGSAuth.AuthenticateCachedUser();

            //Get the configs for this user
            await Configuration.GetConfiguration();

            DiscordOauth2.LaunchOAuth2Prompt();
            
            string code = DiscordOauth2.ListenForAuthorizationCode();

            Debug.Log("Auth Code " + code);
            string tokenJSON = await DiscordOauth2.TokenExchange(code);

            File.WriteAllText(Application.streamingAssetsPath + "/cache/discord.ibxcache", tokenJSON);

        }
    }
}
