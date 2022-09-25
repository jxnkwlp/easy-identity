using System;
using System.Collections.Generic;

namespace EasyIdentity.Models;
/// <summary>
///  Internal class
/// </summary>
public class EasyIdentityToken
{
    public Guid Id { get; set; }
    public string Subject { get; set; }
    public string ClientId { get; set; }
    public string TokenType { get; set; }
    public string TokenName { get; set; }
    public string Issuer { get; set; }
    public string Audiences { get; set; }
    public string Scope { get; set; }
    public int Lifetime { get; set; }
    public DateTime CreationTime { get; set; }
    public Dictionary<string, string> Claims { get; set; }
}
