using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEditor;

namespace Assembly.IBX.Auth
{
    internal static class UGSAuth
    {
        internal static async void AuthenticateCachedUser()
        {
            //If UGS services are not initialized, initialize them
            if(UnityServices.State == ServicesInitializationState.Uninitialized)
            {
                await UnityServices.InitializeAsync();
            }

            //If the user is signed in do not proced to authenticate the cached user, it is not required
            if(AuthenticationService.Instance.IsSignedIn)
            {
                return;
            }

            //If there is a valid session token cached for this user, do not create a new account when authenticating
            if(AuthenticationService.Instance.SessionTokenExists)
            {
                SignInOptions options = new SignInOptions
                {
                    CreateAccount = false
                };

                await AuthenticationService.Instance.SignInAnonymouslyAsync(options);
            }
            else
            {
                SignInOptions options = new SignInOptions
                {
                    CreateAccount = true
                };

                await AuthenticationService.Instance.SignInAnonymouslyAsync(options);
            }
        }
    }
}
