﻿using System;
using System.Security.Claims;

namespace EasyIdentity.Models;

/// <summary>
///  internal class
/// </summary>
public class TokenDescriptor
{
    public TokenDescriptor(string subject, Client client, ClaimsPrincipal principal)
    {
        Guid = Guid.NewGuid();
        Subject = subject;
        Client = client;
        Principal = principal;
    }

    public Guid Guid { get; set; }
    public string Subject { get; }
    public Client Client { get; }
    public ClaimsPrincipal Principal { get; }

    public string TokenType { get; set; } = "Bearer";
    public string TokenName { get; set; }
    public string Issuer { get; set; }
    public string Audiences { get; set; }
    public string Scope { get; set; }
    public TimeSpan Lifetime { get; set; }
    public DateTime CreationTime { get; set; }
}
