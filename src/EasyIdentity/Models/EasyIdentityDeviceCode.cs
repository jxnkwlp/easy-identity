using System;

namespace EasyIdentity.Models;

/// <summary>
///  Internal class
/// </summary>
public class EasyIdentityDeviceCode
{
    public string ClientId { get; set; }
    public string DeviceCode { get; set; }
    public string UserCode { get; set; }
    public DateTime Expiration { get; set; }
    public string Subject { get; set; }
    public bool? Granted { get; set; }
}
