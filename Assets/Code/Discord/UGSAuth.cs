using System.Threading.Tasks;

using Unity.Services.Authentication;
using Unity.Services.Core;

using Assembly.IBX.Remote;

namespace Assembly.IBX.Discord
{
    internal static class UGSAuth
    {
        internal static async Task InitializeAndAuthenticateCachedUserIfRequired()
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
