using System;
using System.IO;

namespace Assembly.IBX.Auth
{
    internal struct APITokenSet
    {
        public string access_token;
        public long expires_in;
        public string refresh_token;
        public string scope;
        public string token_type;
    }

    internal struct APITokenSetWithLocalTimeData
    {
        public APITokenSet token;
        public DateTime initialAuthorizationTime;
        public DateTime latestRefreshTime;
    }

    internal static class OAuth2
    {
        /// <summary>
        /// Checks wether or not a refresh can be performed on a provided oauth2 token
        /// </summary>
        /// <param name="oauth2Token">The oauth2 token to check</param>
        /// <returns>true if the token can be refreshed</returns>
        internal static bool IsAPITokenRefreshAuthorized(APITokenSetWithLocalTimeData oauth2Token)
        {
            TimeSpan t = DateTime.UtcNow - oauth2Token.latestRefreshTime;
            int secondsSinceLastRefresh = (int)t.TotalSeconds;

            return secondsSinceLastRefresh < oauth2Token.token.expires_in;
        }
    }
}
