using System.Security.Claims;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public interface ITokenManager : ITokenCreationService
{
    Task<TokenCreationResult> CreateAsync(string subject, Client client, ClaimsPrincipal principal);

}
