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
            UGSAuth.AuthenticateCachedUser();

            DiscordOauth2 discordOauth2 = new DiscordOauth2();

            discordOauth2.LaunchOAuth2Prompt();
            
            string code = discordOauth2.ListenForAuthorizationCode();

            Debug.Log(code);
            string tokenJSON = await discordOauth2.TokenExchange(code);

            File.WriteAllText(Application.streamingAssetsPath + "/cache/discord.ibxcache", tokenJSON);
        }
    }
}
