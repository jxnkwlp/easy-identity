using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public interface IAuthorizationCodeCreationService
{
    Task<string> CreateAsync(Client client, string[] scopes, string subject, ClaimsPrincipal principal, CancellationToken cancellationToken = default);
}
