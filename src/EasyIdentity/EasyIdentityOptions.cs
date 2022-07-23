using System;

namespace EasyIdentity;

public class EasyIdentityOptions
{
    public const string EasyIdentity = "EasyIdentity";

    public string AuthenticationScheme { get; set; }

    public string Issuer { get; set; }

    public TimeSpan DefaultAccessTokenLifetime { get; set; } = TimeSpan.FromMinutes(5);
    public TimeSpan DefaultRefreshTokenLifetime { get; set; } = TimeSpan.FromDays(30);
    public TimeSpan DefaultIdentityTokenLifetime { get; set; } = TimeSpan.FromMinutes(5);
    public TimeSpan DefaultDeviceCodeLifetime { get; set; } = TimeSpan.FromMinutes(5);
    public TimeSpan DefaultAuthorizationCodeLifetime { get; set; } = TimeSpan.FromMinutes(1);
    public int DefaultDevicePollingInterval { get; set; } = 5;
    public Uri DeviceCodeVerificationUri { get; set; }

    public EasyIdentityOptions()
    {
    }
}
