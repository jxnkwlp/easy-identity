using System.Security.Claims;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public interface IAuthorizationCodeManager
{
    Task<string> GetSubjectAsync(string code);

    Task<string> CreateCodeAsync(Client client, ClaimsPrincipal claimsPrincipal);
}
