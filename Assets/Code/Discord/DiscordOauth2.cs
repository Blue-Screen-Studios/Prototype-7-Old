using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

using UnityEngine;
using Unity.Services.CloudCode;

using Assembly.IBX.Remote;

namespace Assembly.IBX.Discord
{
    public static class DiscordOauth2
    {
        const string SERIALIZATION_FILE_PATH = "/discord.ibx";

        public static void LaunchOAuth2Prompt()
        {
            Application.OpenURL(Configuration.discordConfiguration.authorization_url);
        }

        public static string ListenForAuthorizationCode()
        {   
            HttpListener listener = new HttpListener();
            string code = string.Empty;

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

        /// <summary>
        /// Sends a token exchange request to the Discord API through Cloud Code to prevent decompiling the client secret
        /// </summary>
        /// <param name="authorizationCode">The authorization code to exchange for a token</param>
        /// <returns>A populated APIToken data structure</returns>
        internal static async Task<APITokenSet> TokenExchangeThroughAuthCode(string authorizationCode)
        {
            Dictionary<string, object> arguments = new Dictionary<string, object> { { Configuration.discordConfiguration.token_exchange_param_name, authorizationCode } };
            APITokenSet response = await CloudCodeService.Instance.CallEndpointAsync<APITokenSet>( Configuration.discordConfiguration.token_exchange_endpoint, arguments);

            return response;
        }

        /// <summary>
        /// Sends a token refresh request to the Discord API through Cloud Code to prevent decompiling the client secret
        /// </summary>
        /// <param name="tokenWithLocalData">A reference to a populated APITokenWithLocalTimeData data structure</param>
        /// <returns>A populated APIToken data structure</returns>
        internal static async Task<APITokenSet> RefreshExchange(APITokenSetWithLocalTimeData tokenWithLocalData)
        {
            Dictionary<string, object> arguments = new Dictionary<string, object> { { Configuration.discordConfiguration.refresh_exchange_param_name, tokenWithLocalData.token.refresh_token } };
            APITokenSet response = await CloudCodeService.Instance.CallEndpointAsync<APITokenSet>(Configuration.discordConfiguration.refresh_exchange_endpoint, arguments);

            return response;
        }

        /// <summary>
        /// Populates a new APITokenWithLocalTimeData data structure and serializes it to the serialization file path for use next session
        /// </summary>
        /// <param name="token">The APIToken structure to use as a base for the new APITokenWithLocalTimeData</param>
        /// <param name="setInitialAuthorizationTime">Set this to true to set the initial authorization time to the current time (UTC)</param>
        internal static void CreateAndSerializeAPITokenWithLocalTimeData(APITokenSet token, bool setInitialAuthorizationTime = false)
        {
            APITokenSetWithLocalTimeData serializeableToken = default;

            if(setInitialAuthorizationTime)
            {
                serializeableToken = new APITokenSetWithLocalTimeData
                {
                    token = token,
                    initialAuthorizationTime = DateTime.UtcNow,
                    latestRefreshTime = DateTime.UtcNow
                };
            }
            else
            {
                serializeableToken = new APITokenSetWithLocalTimeData
                {
                    token = token,
                    latestRefreshTime = DateTime.UtcNow
                };
            }

            string json = JsonConvert.SerializeObject(serializeableToken, Formatting.Indented);

            try
            {
                File.WriteAllText(Application.streamingAssetsPath + SERIALIZATION_FILE_PATH, json);
                Debug.Log($"Wrote new serialized token data to {SERIALIZATION_FILE_PATH}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Unable to write file {SERIALIZATION_FILE_PATH} due to an the following exception.");
                Debug.LogException(ex);
            }
        }

        /// <summary>
        /// Reads the token set from disk and desserializes it into a token set data structure
        /// </summary>
        /// <returns>The deserialized TokenSet data structure</returns>
        internal static APITokenSetWithLocalTimeData? DeserializeCachedToken()
        {
            if (File.Exists(Application.streamingAssetsPath + SERIALIZATION_FILE_PATH))
            {
                string json = string.Empty;

                try
                {
                    json = File.ReadAllText(Application.streamingAssetsPath + SERIALIZATION_FILE_PATH);
                    return JsonConvert.DeserializeObject<APITokenSetWithLocalTimeData>(json);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Unable to read file {SERIALIZATION_FILE_PATH} due to the following exception.");
                    Debug.LogException(ex);
                    return null;
                }
            }

            return null;
        }
    }
}
