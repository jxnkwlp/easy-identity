using System;
using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.Extensions.Logging;

namespace EasyIdentity.Services;

public class DeviceCodeGrantTypeHandler : IGrantTypeHandler
{
    public string GrantType => GrantTypesConsts.DeviceCode;

    private readonly ILogger<DeviceCodeGrantTypeHandler> _logger;
    private readonly IUserService _userService;
    private readonly ITokenManager _tokenManager;
    private readonly IDeviceCodeFlowManager _deviceCodeManager;

    public DeviceCodeGrantTypeHandler(ILogger<DeviceCodeGrantTypeHandler> logger, IUserService userService, ITokenManager tokenManager, IDeviceCodeFlowManager deviceCodeManager)
    {
        _logger = logger;
        _userService = userService;
        _tokenManager = tokenManager;
        _deviceCodeManager = deviceCodeManager;
    }

    public async Task<GrantTypeHandledResult> HandleAsync(GrantTypeHandleRequest request)
    {
        var client = request.Client;

        var deviceCode = request.Data.DeviceCode;

        // TODO: check request is too fast
        // need return slow_down message

        if (!await _deviceCodeManager.ValidateDeviceCodeAsync(deviceCode, request.Client))
        {
            _logger.LogDebug($"The device code '{deviceCode}' request validate result: false");
            return GrantTypeHandledResult.Fail(new Exception("expired_token"));
        }

        var subject = await _deviceCodeManager.FindSubjectAsync(deviceCode, request.Client);

        if (string.IsNullOrWhiteSpace(subject))
        {
            return GrantTypeHandledResult.Fail(new Exception("authorization_pending"));
        }

        var isGranted = await _deviceCodeManager.CheckGrantedAsync(deviceCode, request.Client);

        if (!isGranted)
        {
            return GrantTypeHandledResult.Fail(new Exception("authorization_declined"));
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
