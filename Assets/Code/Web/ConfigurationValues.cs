namespace Assembly.IBX.Web
{
    public static partial class Configuration
    {
        #region Remote Config Attributes
        private struct AppAttributes
        {
            public string Oauth2Port;
            public DiscordConfiguration discordConfiguration;
        }

        private struct UserAttributes { }
        #endregion Remote Config Attributes

        #region Serialized JSON Data Structures
        public struct DiscordConfiguration
        {
            public string authorization_url;
            public bool availible_oath2_provider;
            public bool cache_this_configuration_locally;
            public bool cloud_backup_for_tokens;
            public string client_id;
            public string current_user_api_endpoint;
            public string current_user_guilds_api_endpoint;
            public string discord_release_branch_subdomain;
            public string invite_widget_uri;
            public string public_key;
            public string redirect_uri;
            public string refresh_exchange_endpoint;
            public string refresh_exchange_param_name;
            public string success_redirect;
            public string token_exchange_endpoint;
            public string token_exchange_param_name;
        }
        #endregion Serialized JSON Data Structures

        public static int OAuth2Port { get; private set; }
        public static DiscordConfiguration discordConfiguration { get; private set; }
    }
}
