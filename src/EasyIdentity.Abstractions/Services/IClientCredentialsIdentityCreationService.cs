using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public interface IClientCredentialsIdentityCreationService
{
    Task<ClaimsPrincipal> CreateAsync(Client client,CancellationToken cancellationToken=default);
}
