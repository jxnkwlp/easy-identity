namespace EasyIdentity.Endpoints
{
    public static class EndpointProtocolRoutePaths
    {
        public static string DiscoveryConfiguration = ".well-known/openid-configuration";
        public static string DiscoveryWebKeys = DiscoveryConfiguration + "/jwks";

        public static string Authorize = "connect/authorize";
        public static string AuthorizeCallback = Authorize + "/callback";
        public static string Token = "connect/token";
        public static string DeviceCode = "connect/device";

        public static string UserInfo = "connect/userinfo";
        public static string Revocation = "connect/revocation";
        public static string Introspection = "connect/introspect";

        public static string CheckSession = "connect/checksession";
        public static string EndSession = "connect/endsession";
        public static string EndSessionCallback = EndSession + "/callback";
    }
}
