using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.IdentityModel.Tokens;

namespace EasyIdentity.Services;

public class DevelopmentECDsaSigningCredentialsService : ISigningCredentialsService
{
    public Task<List<SigningCredentials>> GetSigningCredentialsAsync(Client client = null, CancellationToken cancellationToken = default)
    {
        var ecdSecurityKey = new ECDsaSecurityKey(ECDsa.Create(ECCurve.NamedCurves.nistP256)) { KeyId = Guid.NewGuid().ToString("N") };
        var credentials = new SigningCredentials(ecdSecurityKey, SecurityAlgorithms.EcdsaSha256);

        return Task.FromResult(new List<SigningCredentials> { credentials });
    }
}
