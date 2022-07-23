using System.Collections.Generic;

namespace EasyIdentity.Models;

public class DeviceCodeData
{
    public string DeviceCode { get; set; }
    public string UserCode { get; set; }
    public string VerificationUri { get; set; }
    public int Interval { get; set; }
    public int ExpiresIn { get; set; }
    public string Message { get; set; }

    public Dictionary<string, object> ToDictionary()
    {
        return new Dictionary<string, object> {
                { "device_code", DeviceCode },
                { "user_code", UserCode },
                { "verification_uri", VerificationUri },
                { "interval", Interval },
                { "expires_in", ExpiresIn },
                { "message", Message },
            };
    }
}
