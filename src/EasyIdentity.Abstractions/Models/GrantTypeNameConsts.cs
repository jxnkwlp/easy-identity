namespace EasyIdentity.Models;

public static class GrantTypeNameConsts
{
    public const string ClientCredentials = "client_credentials";
    public const string AuthorizationCode = "authorization_code";
    public const string Password = "password";
    public const string Implicit = "implicit";
    public const string RefreshToken = "refresh_token";
    public const string DeviceCode = "urn:ietf:params:oauth:grant-type:device_code";
    public const string Ciba = "urn:openid:params:grant-type:ciba";

    public static string[] All = new string[] { AuthorizationCode, ClientCredentials, DeviceCode, Implicit, Password, RefreshToken, Ciba };
}
