using System;

namespace EasyIdentity.Models;

/// <summary>
///  Internal class
/// </summary>
public class EasyIdentityAuthorizationCode
{
    public string ClientId { get; set; }

    public string Code { get; set; }

    public string Subject { get; set; }

    public DateTime Expiration { get; set; }

    public string RedirectUrl { get; set; }

    public string CodeChallenge { get; set; }

    public string CodeChallengeMethod { get; set; }
}
