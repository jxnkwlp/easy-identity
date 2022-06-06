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

        public async Task HandleAsync(HttpContext context)
        {
            var requestData = await _paramReader.ReadAsync();

            var validationResult = await _deviceCodeRequestValidator.ValidateAsync(requestData);

            // TODO 

            var data = new
            {
                device_code = Guid.NewGuid(),
                user_code = Guid.NewGuid(),
                verification_uri = "https://example.okta.com/device",
                interval = 5,
                expires_in = 1800,
            };

            var jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web) { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull };
            await context.Response.WriteAsJsonAsync(data, jsonSerializerOptions);
        }
    }
}
