using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public class ClientCredentialsGrantTypeHandler : IGrantTypeHandler
{
    public string GrantType => GrantTypesConsts.ClientCredentials;

    private readonly ITokenManager _tokenManager;
    private readonly IClientCredentialsIdentityCreationService _clientCredentialsIdentityCreationService;

    public ClientCredentialsGrantTypeHandler(ITokenManager tokenManager, IClientCredentialsIdentityCreationService clientCredentialsIdentityCreationService)
    {
        _tokenManager = tokenManager;
        _clientCredentialsIdentityCreationService = clientCredentialsIdentityCreationService;
    }

    public async Task<GrantTypeHandledResult> HandleAsync(GrantTypeHandleRequest context)
    {
        var client = context.Client;

        var identity = await _clientCredentialsIdentityCreationService.CreateAsync(client);

        var token = await _tokenManager.CreateAsync(client.ClientId, client, identity);

        return GrantTypeHandledResult.Success(new TokenData
        {
            AccessToken = token.AccessToken,
            RefreshToken = token.RefreshToken,
            Scope = string.Join(" ", client.Scopes),
            ExpiresIn = (int)token.TokenDescriptor.Lifetime.TotalSeconds,
            TokenType = token.TokenDescriptor.TokenType,
        });
    }
}
