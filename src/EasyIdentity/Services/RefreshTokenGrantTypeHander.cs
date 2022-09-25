using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public class RefreshTokenGrantTypeHander : IGrantTypeHandler
{
    public string GrantType => GrantTypeNameConsts.RefreshToken;

    private readonly ITokenManager _tokenManager;
    private readonly IUserService _userService;

    public RefreshTokenGrantTypeHander(ITokenManager tokenManager, IUserService userService)
    {
        _tokenManager = tokenManager;
        _userService = userService;
    }

    public async Task<GrantTypeExecutionResult> ExecuteAsync(GrantTypeExecutionRequest request, CancellationToken cancellationToken = default)
    {
        var client = request.Client;
        var scopes = request.Data.Scopes;
        var requestData = request.Data;

        var subject = await _tokenManager.GetRefreshTokenSubjectAsync(request.Data.RefreshToken, cancellationToken);

        if (string.IsNullOrWhiteSpace(subject))
            return GrantTypeExecutionResult.Fail(new Exception("invalid_user"));

        // scopes 
        var tokenScopes = await _tokenManager.GetRefreshTokenScopesAsync(request.Data.RefreshToken, cancellationToken);

        if (scopes?.Any() == true && scopes.Except(tokenScopes).Count() > 0)
        {
            return GrantTypeExecutionResult.Fail(new Exception("invalid_scopes"));
        }

        var userProfile = await _userService.GetProfileAsync(new UserProfileRequest(client, subject, request.Data), cancellationToken);

        if (userProfile.Locked)
            return GrantTypeExecutionResult.Fail(new Exception("access_denied"));

        var token = await _tokenManager.CreateAsync(client, scopes, subject, userProfile.Principal, request.Data, cancellationToken);

        return GrantTypeExecutionResult.Success(token);
    }
}
