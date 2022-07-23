using System.Collections.Generic;

namespace EasyIdentity.Models;

public class RequestData
{
    public Dictionary<string, string> Data { get; }

    public RequestData(Dictionary<string, string> data)
    {
        Data = data;
    }

    public string this[string key]
    {
        get
        {
            Data.TryGetValue(key, out string value);
            return value;
        }
    }

    /// <summary>
    ///  'response_type'
    /// </summary>
    public string ResponseType => this["response_type"];
    public string GrantType => this["grant_type"];
    public string ClientId => this["client_id"];
    public string ClientSecret => this["client_secret"];
    public string Authorization => this["authorization"];
    public string Scope => this["scope"];
    public string Code => this["code"];
    public string RedirectUri => this["redirect_uri"];
    public string State => this["state"];
    public string Nonce => this["nonce"];
    public string Username => this["username"];
    public string Password => this["password"];
    public string DeviceCode => this["device_code"];

}
