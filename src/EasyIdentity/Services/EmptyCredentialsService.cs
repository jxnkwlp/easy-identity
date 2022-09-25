using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.IdentityModel.Tokens;

namespace EasyIdentity.Services;

public class EmptyCredentialsService : ICredentialsService
{
    public Task<List<EncryptingCredentials>> GetEncryptingCredentialsAsync(Client client = null, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<List<EncryptingCredentials>>(default);
    }

    public Task<List<SigningCredentials>> GetSigningCredentialsAsync(Client client = null, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<List<SigningCredentials>>(default);
    }
}
