﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.IdentityModel.Tokens;

namespace EasyIdentity.Services;

public class DevelopmentECDsaCredentialsService : ICredentialsService
{
    public Task<List<EncryptingCredentials>> GetEncryptingCredentialsAsync(Client client = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<SigningCredentials>> GetSigningCredentialsAsync(Client client = null, CancellationToken cancellationToken = default)
    {
        var ecdSecurityKey = new ECDsaSecurityKey(ECDsa.Create(ECCurve.NamedCurves.nistP256)) { KeyId = Guid.NewGuid().ToString("N") };
        var credentials = new SigningCredentials(ecdSecurityKey, SecurityAlgorithms.EcdsaSha256);

        return Task.FromResult(new List<SigningCredentials> { credentials });
    }
}
