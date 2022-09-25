using System;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public class PasswordGrantTypeHandler : IGrantTypeHandler
{
    public string GrantType => GrantTypeNameConsts.Password;

    private readonly IUserService _userService;
    private readonly ITokenManager _tokenManager;

    public PasswordGrantTypeHandler(IUserService userService, ITokenManager tokenManager)
    {
        _userService = userService;
        _tokenManager = tokenManager;
    }

    public async Task<GrantTypeExecutionResult> ExecuteAsync(GrantTypeExecutionRequest request, CancellationToken cancellationToken = default)
    {
        var client = request.Client;
        var scopes = request.Data.Scopes;
        var requestData = request.Data;

        var subject = await _userService.GetSubjectAsync(requestData.Username, requestData.Password, requestData);

        if (string.IsNullOrWhiteSpace(subject))
        {
            return GrantTypeExecutionResult.Fail(new Exception("invalid_username"));
        }

        var userProfile = await _userService.GetProfileAsync(new UserProfileRequest(client, subject, request.Data));

        if (userProfile.Locked)
        {
            return GrantTypeExecutionResult.Fail(new Exception("access_denied"));
        }

        var token = await _tokenManager.CreateAsync(client, scopes, subject, userProfile.Principal,  request.Data);

        return GrantTypeExecutionResult.Success(token);
    }

}
