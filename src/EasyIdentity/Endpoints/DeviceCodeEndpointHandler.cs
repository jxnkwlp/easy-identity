using System.Threading.Tasks;
using EasyIdentity.Models;
using EasyIdentity.Services;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Endpoints;

public class DeviceCodeEndpointHandler : IEndpointHandler
{
    public string Path => EndpointProtocolRoutePaths.DeviceCode;

    public string[] Methods => new string[] { HttpMethods.Post };

    private readonly IRequestParamReader _requestParamReader;
    private readonly IDeviceCodeRequestValidator _deviceCodeRequestValidator;
    private readonly IDeviceCodeFlowManager _deviceCodeManager;
    private readonly IResponseWriter _responseWriter;

    public DeviceCodeEndpointHandler(IRequestParamReader requestParamReader, IDeviceCodeRequestValidator deviceCodeRequestValidator, IDeviceCodeFlowManager deviceCodeManager, IResponseWriter responseWriter)
    {
        _requestParamReader = requestParamReader;
        _deviceCodeRequestValidator = deviceCodeRequestValidator;
        _deviceCodeManager = deviceCodeManager;
        _responseWriter = responseWriter;
    }

    public async Task HandleAsync(HttpContext context)
    {
        var requestData = await _requestParamReader.ReadAsync();

        var validationResult = await _deviceCodeRequestValidator.ValidateAsync(requestData);

        if (!validationResult.Succeeded)
        {
            await _responseWriter.WriteAsync(new ResponseDescriptor(requestData, validationResult.Error, validationResult.ErrorDescription));
            return;
        }

        var codeRequestResult = await _deviceCodeManager.RequestAsync(requestData, validationResult);

        await _responseWriter.WriteAsync(new ResponseDescriptor(requestData, codeRequestResult.ToDictionary()));
    }
}
