namespace Assembly.IBX.Auth
{
    internal struct Token
    {
        public string access_token;
        public long expires_in;
        public string refresh_token;
        public string scope;
        public string token_type;
    }

    internal interface IOAuth2
    {
        public string launch_url { get; set; }
        public string redirect_uri_incoming { get; set; }
        public string redirect_url_outbound { get; set; }
        public short port { get; set; }

        /// <summary>
        /// Implement the code to launch the OAuth2 Prompt
        /// </summary>
        public abstract void LaunchOAuth2Prompt();

        /// <summary>
        /// Implement the code to listen for an authorization code
        /// </summary>
        /// <returns>The authorization code</returns>
        public abstract string ListenForAuthorizationCode();

        /// <summary>
        /// Implement the token exchange
        /// </summary>
        /// <param name="authorizationCode">The authorization code</param>
        /// <returns>The token</return`s>
        public virtual string TokenExchange(string authorizationCode)
        {
            throw new System.NotImplementedException();
        }
    }
}
