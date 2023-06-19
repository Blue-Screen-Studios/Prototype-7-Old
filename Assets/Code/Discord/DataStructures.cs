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

    internal struct CDNResource
    {
        public enum ResourceType { AVATAR, BANNER, FILE };
        public ResourceType resourceType;

        public string name;
        public string extension;
    }
}
