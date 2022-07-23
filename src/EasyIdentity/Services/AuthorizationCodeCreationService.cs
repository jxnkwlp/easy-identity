using System;
using System.Security.Claims;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public class AuthorizationCodeCreationService : IAuthorizationCodeCreationService
{
    public Task<string> CreateAsync(Client client, ClaimsPrincipal principal)
    {
        string code = Guid.NewGuid().ToString("N");

        return Task.FromResult(code);
    }
}
