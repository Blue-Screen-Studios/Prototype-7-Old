using System;
using System.IO;

namespace Assembly.IBX.Discord
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

    internal struct DiscordUser
    {
        public string id;
        public string username;
        public string global_name;
        public string avatar;
        public string discriminator;
        public int public_flags;
        public int flags;
        public string banner;
        public string banner_color;
        public int accent_color;
        public string locale;
        public bool mfa_enabled;
        public int premium_type;
        public string avatar_decoration;
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
