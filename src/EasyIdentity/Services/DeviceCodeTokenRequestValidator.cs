using System;
using System.Linq;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public class DeviceCodeTokenRequestValidator : IGrantTypeTokenRequestValidator
{
    public string GrantType => GrantTypesConsts.DeviceCode;

    private readonly IClientManager _clientManager;

    public DeviceCodeTokenRequestValidator(IClientManager clientManager)
    {
        _clientManager = clientManager;
    }

    public async Task<RequestValidationResult> ValidateAsync(RequestData data)
    {
        var grantType = data.GrantType;
        var clientId = data.ClientId;
        var deviceCode = data.DeviceCode;

        if (string.IsNullOrEmpty(clientId))
            return RequestValidationResult.Fail("invalid_request");

        var client = await _clientManager.FindByClientIdAsync(clientId);

        if (client == null)
            return RequestValidationResult.Fail("invalid_client", "Invalid client Id.");

        if (client.GrantTypes.Contains(grantType) == false)
            return RequestValidationResult.Fail("unsupported_grant_type", "Invalid grant type.");

        if (string.IsNullOrEmpty(deviceCode))
            return RequestValidationResult.Fail("invalid_request", "Invalid deviceCode.");

        return RequestValidationResult.Success(client, data, grantType);
    }
}
