using System;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public class AuthorizationCodeGrantTypeHandler : IGrantTypeHandler
{
    public string GrantType => GrantTypesConsts.AuthorizationCode;

    private readonly IUserService _userService;
    private readonly IAuthorizationCodeManager _authorizationCodeManager;
    private readonly ITokenManager _tokenManager;

    public AuthorizationCodeGrantTypeHandler(IUserService userService, IAuthorizationCodeManager authorizationCodeManager, ITokenManager tokenManager)
    {
        _userService = userService;
        _authorizationCodeManager = authorizationCodeManager;
        _tokenManager = tokenManager;
    }

    public async Task<GrantTypeHandledResult> HandleAsync(GrantTypeHandleRequest request)
    {
        var client = request.Client;

        var code = request.Data.Code;

        var subject = await _authorizationCodeManager.GetSubjectAsync(code);

        if (string.IsNullOrWhiteSpace(subject))
        {
            return GrantTypeHandledResult.Fail(new Exception("invalid_code"));
        }

        var userProfile = await _userService.GetProfileAsync(new UserProfileRequest(client, subject, request.Data));

        if (userProfile.Locked)
        {
            return GrantTypeHandledResult.Fail(new Exception("access_denied"));
        }

        var token = await _tokenManager.CreateAsync(subject, client, userProfile.Principal);

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
