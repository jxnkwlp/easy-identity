using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.IdentityModel.Tokens;

namespace EasyIdentity.Services;

public class DevelopmentRSASigningCredentialsStore : ISigningCredentialsStore
{
    public Task<List<SigningCredentials>> GetSigningCredentialsAsync(Client client)
    {
        var rsaSecurityKey = new RsaSecurityKey(RSA.Create(2048)) { KeyId = Guid.NewGuid().ToString("N") };
        var credentials = new SigningCredentials(rsaSecurityKey, SecurityAlgorithms.RsaSha256);

        return Task.FromResult(new List<SigningCredentials> { credentials });
    }
}
