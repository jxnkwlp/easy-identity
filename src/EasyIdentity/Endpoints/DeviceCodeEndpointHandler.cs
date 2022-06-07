using System;
using System.Text.Json;
using System.Threading.Tasks;
using EasyIdentity.Services;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Endpoints
{
    public class DeviceCodeEndpointHandler : IEndpointHandler
    {
        public string Path => ProtocolRoutePaths.DeviceCode;

        public string[] Methods => new string[] { HttpMethods.Post };

        private readonly IRequestParamReader _paramReader;
        private readonly IDeviceCodeRequestValidator _deviceCodeRequestValidator;
        private readonly IDeviceCodeService _deviceCodeService; 

        public DeviceCodeEndpointHandler(IRequestParamReader paramReader, IDeviceCodeRequestValidator deviceCodeRequestValidator, IDeviceCodeService deviceCodeService)
        {
            _paramReader = paramReader;
            _deviceCodeRequestValidator = deviceCodeRequestValidator;
            _deviceCodeService = deviceCodeService;
        }

        public async Task HandleAsync(HttpContext context)
        {
            var requestData = await _paramReader.ReadAsync();

            var validationResult = await _deviceCodeRequestValidator.ValidateAsync(requestData);

            var codeRequestResult = await _deviceCodeService.CodeRequestAsync(requestData, validationResult);

            var jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web) { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull };

            await context.Response.WriteAsJsonAsync(codeRequestResult, jsonSerializerOptions); 
        }
    }
}
