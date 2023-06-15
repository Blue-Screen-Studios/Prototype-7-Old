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
}
