using System;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public class AuthorizationCodeGrantTypeHandler : IGrantTypeHandler
{
    public string GrantType => GrantTypeNameConsts.AuthorizationCode;

    private readonly IUserService _userService;
    private readonly IAuthorizationCodeFlowManager _authorizationCodeManager;
    private readonly ITokenManager _tokenManager;

    public AuthorizationCodeGrantTypeHandler(IUserService userService, IAuthorizationCodeFlowManager authorizationCodeManager, ITokenManager tokenManager)
    {
        _userService = userService;
        _authorizationCodeManager = authorizationCodeManager;
        _tokenManager = tokenManager;
    }

    public async Task<GrantTypeExecutionResult> ExecuteAsync(GrantTypeExecutionRequest request, CancellationToken cancellationToken = default)
    {
        var client = request.Client;
        var scopes = request.Data.Scopes;
        var code = request.Data.Code;

        var subject = await _authorizationCodeManager.GetSubjectAsync(client, scopes, code, cancellationToken);

        if (string.IsNullOrWhiteSpace(subject))
        {
            return GrantTypeExecutionResult.Fail(new Exception("invalid_code"));
        }

        var userProfile = await _userService.GetProfileAsync(new UserProfileRequest(client, subject, request.Data), cancellationToken);

        if (userProfile.Locked)
        {
            return GrantTypeExecutionResult.Fail(new Exception("access_denied"));
        }

        var token = await _tokenManager.CreateAsync( client, scopes, subject, userProfile.Principal, request.Data);

        return GrantTypeExecutionResult.Success(token);
    }
}
