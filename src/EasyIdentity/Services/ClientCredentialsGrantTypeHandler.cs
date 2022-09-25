using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public class ClientCredentialsGrantTypeHandler : IGrantTypeHandler
{
    public string GrantType => GrantTypeNameConsts.ClientCredentials;

    private readonly ITokenManager _tokenManager;
    private readonly IClientCredentialsIdentityCreationService _clientCredentialsIdentityCreationService;

    public ClientCredentialsGrantTypeHandler(ITokenManager tokenManager, IClientCredentialsIdentityCreationService clientCredentialsIdentityCreationService)
    {
        _tokenManager = tokenManager;
        _clientCredentialsIdentityCreationService = clientCredentialsIdentityCreationService;
    }

    public async Task<GrantTypeExecutionResult> ExecuteAsync(GrantTypeExecutionRequest request, CancellationToken cancellationToken = default)
    {
        var client = request.Client;
        var scopes = request.Data.Scopes;

        var claimsPrincipal = await _clientCredentialsIdentityCreationService.CreateAsync(client);

        var token = await _tokenManager.CreateAsync(client, scopes, client.ClientId, claimsPrincipal, request.Data);

        return GrantTypeExecutionResult.Success(token);
    }
}
