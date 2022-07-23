using System.Collections.Generic;
using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.IdentityModel.Tokens;

namespace EasyIdentity.Services;

public class EmptySigningCredentialsStore : ISigningCredentialsStore
{
    public Task<List<SigningCredentials>> GetSigningCredentialsAsync(Client client=null)
    {
        return Task.FromResult<List<SigningCredentials>>(default);
    }
}
