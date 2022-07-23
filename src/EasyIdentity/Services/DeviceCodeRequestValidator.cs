using System.Threading.Tasks;
using EasyIdentity.Models;
using EasyIdentity.Stores;

namespace EasyIdentity.Services;

public class DeviceCodeRequestValidator : IDeviceCodeRequestValidator
{
    private readonly IClientStore _clientStore;

    public DeviceCodeRequestValidator(IClientStore clientStore)
    {
        _clientStore = clientStore;
    }

    public async Task<RequestValidationResult> ValidateAsync(RequestData requestData)
    {
        var clientId = requestData["client_id"];
        var clientSecret = requestData["client_secret"];
        var authorization = requestData["authorization"];

        if (string.IsNullOrEmpty(clientId))
            return RequestValidationResult.Fail("invalid_request");

        var client = await _clientStore.FindClientAsync(clientId);

        if (client == null)
            return RequestValidationResult.Fail("invalid_client", "Invalid client Id.");

        return RequestValidationResult.Success(client, requestData);
    }
}
