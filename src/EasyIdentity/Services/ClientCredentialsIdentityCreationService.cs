using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public class ClientCredentialsIdentityCreationService : IClientCredentialsIdentityCreationService
{
    public Task<ClaimsPrincipal> CreateAsync(Client client, CancellationToken cancellationToken = default)
    {
        var identity = new ClaimsIdentity();
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, client.ClientId));
        identity.AddClaim(new Claim(ClaimTypes.Name, client.DisplayName));

        return Task.FromResult(new ClaimsPrincipal(identity));
    }
}
