using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Assembly.IBX.Main
{
    public class Test1 : MonoBehaviour
    {
        private async void Start()
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            DiscordOauth2 discordOauth2 = new DiscordOauth2();

            discordOauth2.LaunchOAuth2Prompt();
            
            string code = discordOauth2.ListenForAuthorizationCode();

            Debug.Log(code);
            string x = await discordOauth2.TokenExchange(code);
        }
    }
}
