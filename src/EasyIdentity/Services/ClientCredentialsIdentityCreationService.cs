using System.Security.Claims;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public class ClientCredentialsIdentityCreationService : IClientCredentialsIdentityCreationService
{
    public Task<ClaimsPrincipal> CreateAsync(Client client)
    {
        var identity = new ClaimsIdentity();
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, client.ClientId));
        identity.AddClaim(new Claim(ClaimTypes.Name, client.ClientName));

        return Task.FromResult(new ClaimsPrincipal(identity));
    }
}
