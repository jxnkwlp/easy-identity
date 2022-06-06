namespace EasyIdentity.Endpoints
{
    public static class ProtocolRoutePaths
    {
        public const string DiscoveryConfiguration = ".well-known/openid-configuration";
        public const string DiscoveryWebKeys = DiscoveryConfiguration + "/jwks";

        public const string Authorize = "connect/authorize";
        public const string AuthorizeCallback = Authorize + "/callback";
        public const string Token = "connect/token";
        public const string DeviceCode = "connect/device";

        public const string Revocation = "connect/revocation";
        public const string UserInfo = "connect/userinfo";
        public const string Introspection = "connect/introspect";

        public const string CheckSession = "connect/checksession";
        public const string EndSession = "connect/endsession";
        public const string EndSessionCallback = EndSession + "/callback";
    }
}
