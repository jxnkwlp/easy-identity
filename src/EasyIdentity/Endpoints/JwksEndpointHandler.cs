using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using JsonWebKey = EasyIdentity.Models.JsonWebKey;

namespace EasyIdentity.Endpoints
{
    public class JwksEndpointHandler : IEndpointHandler
    {
        public string Path => EndpointProtocolRoutePaths.DiscoveryWebKeys;

        public string[] Methods => new string[] { HttpMethods.Get };

        private readonly EasyIdentityOptions _easyIdentityOptions;

        public JwksEndpointHandler(EasyIdentityOptions easyIdentityOptions)
        {
            _easyIdentityOptions = easyIdentityOptions;
        }

        public async Task HandleAsync(HttpContext context)
        {
            var list = new List<JsonWebKey>();

            foreach (var item in _easyIdentityOptions.SigningCredentials)
            {
                JsonWebKey webKey = new JsonWebKey()
                {
                    Kid = item.Kid,
                    Use = "sig",
                };

                if (item.Key is RsaSecurityKey rsaSecurityKey)
                {
                    var parameters = rsaSecurityKey.Rsa.ExportParameters(false);

                    webKey.Kty = "RSA";
                    webKey.Alg = item.Algorithm;
                    webKey.N = Convert.ToBase64String(parameters.Modulus);
                    webKey.E = Convert.ToBase64String(parameters.Exponent);
                }

                list.Add(webKey);
            }

            await context.Response.WriteAsJsonAsync(new { keys = list }, new JsonSerializerOptions(JsonSerializerDefaults.Web) { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull });
        }
    }
}
