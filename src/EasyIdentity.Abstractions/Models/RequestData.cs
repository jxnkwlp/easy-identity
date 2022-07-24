using System.Collections.Generic;

namespace EasyIdentity.Models;

public class RequestData
{
    public IReadOnlyDictionary<string, string> Data { get; }

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
    ///  'grant_type'
    /// </summary>
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
    public string Tenant => this["tenant"];
    public string Prompt => this["prompt"];
    public string LoginHint => this["login_hint"];
    public string DomainHint => this["domain_hint"];
    public string CodeChallenge => this["code_challenge"];
    public string CodeChallengeMethod => this["code_challenge_method"];
    public string CodeVerifier => this["code_verifier"];
    public string ClientAssertionType => this["client_assertion_type"];
    public string ClientAssertion => this["client_assertion"];

    /// <summary>
    ///  'response_type'
    /// </summary>
    public string ResponseType => this["response_type"];

    /// <summary>
    ///  'response_mode'
    /// </summary>
    /// <remarks>
    ///  fragment/form_post/query
    /// </remarks>
    public string ResponseMode => this["response_mode"];

}
