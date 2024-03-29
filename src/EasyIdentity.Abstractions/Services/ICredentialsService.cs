﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.IdentityModel.Tokens;

namespace EasyIdentity.Services;

public interface ICredentialsService
{
    Task<List<SigningCredentials>> GetSigningCredentialsAsync(Client client = null, CancellationToken cancellationToken = default);

    Task<List<EncryptingCredentials>> GetEncryptingCredentialsAsync(Client client = null, CancellationToken cancellationToken = default);
}
