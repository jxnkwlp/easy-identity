using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EasyIdentity.Services;

public class RefreshTokenRequestValidator : IGrantTypeTokenRequestValidator
{
    public string GrantType => GrantTypeNameConsts.RefreshToken;

    private readonly ILogger<RefreshTokenRequestValidator> _logger;
    private readonly IClientAuthenticationService _clientAuthenticationService;
    private readonly ITokenManager _tokenManager;
    private IHttpContextAccessor _httpContextAccessor;
    private EasyIdentityOptions _identityOptions;

    public RefreshTokenRequestValidator(ILogger<RefreshTokenRequestValidator> logger, IClientAuthenticationService clientAuthenticationService, ITokenManager tokenManager, IHttpContextAccessor httpContextAccessor, IOptions<EasyIdentityOptions> identityOptions)
    {
        _logger = logger;
        _clientAuthenticationService = clientAuthenticationService;
        _tokenManager = tokenManager;
        _httpContextAccessor = httpContextAccessor;
        _identityOptions = identityOptions.Value;
    }

    public async Task<RequestValidationResult> ValidateAsync(RequestData data)
    {
        // see: https://www.oauth.com/oauth2-servers/access-tokens/refreshing-access-tokens/

        var clientId = data.ClientId;
        var grantType = data.GrantType;
        var refreshToken = data.RefreshToken;
        var scopes = data.Scopes;

        var clientAuthenticationResult = await _clientAuthenticationService.ValidateAsync(clientId, grantType, data);

        if (!clientAuthenticationResult.Succeeded)
        {
            var error = clientAuthenticationResult.Error;
            return RequestValidationResult.Fail(error.Error, error.Description);
        }

        if (string.IsNullOrWhiteSpace(refreshToken))
            return RequestValidationResult.Fail("invalid_request", "Request was missing the 'refresh_token' parameter");

        //
        if (!await _tokenManager.ValidateRefreshTokenAsync(refreshToken, scopes))
            return RequestValidationResult.Fail("invalid_grant", "Request refresh token invalid");

        return RequestValidationResult.Success(clientAuthenticationResult.Client, data, grantType);
    }
}
