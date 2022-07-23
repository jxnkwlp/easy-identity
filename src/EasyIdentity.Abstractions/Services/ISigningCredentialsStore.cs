using System.Collections.Generic;
using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.IdentityModel.Tokens;

namespace EasyIdentity.Services;

public interface ISigningCredentialsStore
{
    Task<List<SigningCredentials>> GetSigningCredentialsAsync(Client client = null);
}
