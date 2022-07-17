using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EasyIdentity.Services
{
    public class AuthorizationInteractionService : IAuthorizationInteractionService
    {
        public Task<bool> DeviceUserCodeAuthorizationAsync(string userCode, ClaimsPrincipal claimsPrincipal, bool grant)
        {
            throw new NotImplementedException();
        }
    }
}
