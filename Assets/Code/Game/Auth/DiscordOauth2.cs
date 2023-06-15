using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using Unity.Services.CloudCode;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Assembly.IBX.Main
{
    public class DiscordOauth2 : OAuth2
    {
        public string launch_url { get => "https://discord.com/api/oauth2/authorize?client_id=1116970853905743894&redirect_uri=http%3A%2F%2Flocalhost%3A6449%2Fibx%2Fdiscord&response_type=code&scope=identify%20guilds.join%20guilds"; set => value = "https://discord.com/api/oauth2/authorize?client_id=1116970853905743894&redirect_uri=http%3A%2F%2Flocalhost%3A6449%2Fibx%2Fdiscord&response_type=code&scope=identify%20guilds.join%20guilds"; }
        public string redirect_uri_incoming { get => "http://localhost:6449/ibx/discord/"; set => value = "http://localhost:6449/ibx/discord/"; }
        public string redirect_url_outbound { get => "https://discord.com/oauth2/authorized"; set => value = "https://discord.com/oauth2/authorized"; }
        public short port { get => 6449; set => value = 6449; }

        public void LaunchOAuth2Prompt()
        {
            Application.OpenURL(launch_url);
        }

        public string ListenForAuthorizationCode()
        {   
            HttpListener listener = new HttpListener();
            string code = string.Empty;

            listener.Prefixes.Add(redirect_uri_incoming);

            listener.Start();

            Debug.Log($"Listening for Discord OAuth2 on {redirect_url_outbound}");

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
                    context.Response.Headers.Add("Location", redirect_url_outbound);

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

        public async Task<string> TokenExchange(string authorizationCode)
        {
            Dictionary<string, object> arguments = new Dictionary<string, object> { { "code", authorizationCode } };
            OAuth2TokenResponse response = await CloudCodeService.Instance.CallEndpointAsync<OAuth2TokenResponse>("DiscordOauth2", arguments);

            Debug.Log(JsonConvert.SerializeObject(response));

            return JsonConvert.SerializeObject(response);

        }
    }

    public class OAuth2TokenResponse
    {
        public string access_token;
        public long expires_in;
        public string refresh_token;
        public string scope;
        public string token_type;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("access_token ");
            sb.Append(access_token);
            sb.Append("| expires_in ");
            sb.Append(expires_in);
            sb.Append("| refresh_token ");
            sb.Append(refresh_token);
            sb.Append("| scope ");
            sb.Append(scope);
            sb.Append("| token_type ");
            sb.Append(token_type);

            return sb.ToString();
        }

    }

}
