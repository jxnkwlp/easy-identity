using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EasyIdentity.Services
{
    public interface IAuthorizationCodeStoreService
    {
        Task CreateAsync(ClaimsPrincipal principal, string code, DateTime expiration);

        Task<string> GetSubjectAsync(string code);

        Task RemoveAsync(string code);
    }
}
