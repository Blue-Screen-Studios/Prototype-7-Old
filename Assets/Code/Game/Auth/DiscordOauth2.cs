using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;

using UnityEngine;
using Unity.Services.CloudCode;

using Assembly.IBX.Remote;

namespace Assembly.IBX.Auth
{
    public static class DiscordOauth2
    {
        public static void LaunchOAuth2Prompt()
        {
            Application.OpenURL(Configuration.discordConfiguration.authorization_url);
        }

        public static string ListenForAuthorizationCode()
        {   
            HttpListener listener = new HttpListener();
            string code = string.Empty;

            Debug.Log($"http://localhost:{Configuration.OAuth2Port}{Configuration.discordConfiguration.redirect_uri}");

            listener.Prefixes.Add($"http://localhost:{Configuration.OAuth2Port}{Configuration.discordConfiguration.redirect_uri}");

            listener.Start();

            Debug.Log($"Listening for Discord OAuth2 on {$"http://localhost:{Configuration.OAuth2Port}{Configuration.discordConfiguration.redirect_uri}"}");

            try
            {
                bool validRequestRecieved = false;

                while (!validRequestRecieved)
                {
                    //Wait for an incoming request
                    HttpListenerContext context = listener.GetContext();

                    //Parse the incoming request

                    code = context.Request.QueryString["code"];
                    Debug.Log($"Discord Authorization Code: {code}");

                    //Respond to the request
                    context.Response.StatusCode = (int)HttpStatusCode.Redirect;
                    context.Response.Headers.Add("Location", Configuration.discordConfiguration.success_redirect);

                    //close the response
                    context.Response.Close();

                    //Exit the loop if needed
                    validRequestRecieved = !string.IsNullOrEmpty(code);
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
            finally
            {
                listener.Stop();
            }

            return code;
        }

        public static async Task<string> TokenExchange(string authorizationCode)
        {
            Dictionary<string, object> arguments = new Dictionary<string, object> { { Configuration.discordConfiguration.token_exchange_param_name, authorizationCode } };
            Token response = await CloudCodeService.Instance.CallEndpointAsync<Token>( Configuration.discordConfiguration.token_exchange_endpoint, arguments);

            Debug.Log(JsonConvert.SerializeObject(response));
            return JsonConvert.SerializeObject(response);
        }
    }
}
