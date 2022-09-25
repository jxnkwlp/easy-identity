using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public class ImplicitGrantTypeHandler : IGrantTypeHandler
{
    public string GrantType => GrantTypeNameConsts.Implicit;

    private readonly IUserService _userService;
    private readonly ITokenManager _tokenManager;
    private readonly IAuthorizationCodeFlowManager _authorizationCodeManager;

    public ImplicitGrantTypeHandler(IUserService userService, ITokenManager tokenManager, IAuthorizationCodeFlowManager authorizationCodeManager)
    {
        _userService = userService;
        _tokenManager = tokenManager;
        _authorizationCodeManager = authorizationCodeManager;
    }

    public async Task<GrantTypeExecutionResult> ExecuteAsync(GrantTypeExecutionRequest request, CancellationToken cancellationToken = default)
    {
        var client = request.Client;
        var scopes = request.Data.Scopes;
        var subject = request.Subject;
        var responseTypes = request.Data.ResponseTypes;

        var userProfile = await _userService.GetProfileAsync(new UserProfileRequest(client, subject, request.Data));

        if (userProfile.Locked)
        {
            return GrantTypeExecutionResult.Fail(new Exception("access_denied"));
        }

        var urlData = new Dictionary<string, object>();
        if (responseTypes.Contains("code"))
        {
            var code = await _authorizationCodeManager.CreateCodeAsync(client, scopes, subject, request.ClaimsPrincipal, request.Data, cancellationToken);

            urlData["code"] = code;
        }

        var token = await _tokenManager.CreateAsync(client, scopes, subject, userProfile.Principal, request.Data);

        urlData["token_type"] = "Bearer";
        urlData["access_token"] = token.AccessToken;
        urlData["scope"] = string.Join(" ", client.Scopes);
        urlData["state"] = request.Data.State;
        urlData["expires_in"] = (int)token.AccessTokenDescriptor.Lifetime.TotalSeconds;

        // TODO
        // response_modes

        string url = $"{request.Data.RedirectUri}?#{string.Join("&", urlData.Select(x => $"{x.Key}={x.Value}"))}";

        return GrantTypeExecutionResult.Success(url);
    }
}
