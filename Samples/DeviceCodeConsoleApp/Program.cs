using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.IdentityModel.Logging;

namespace DeviceCodeConsoleApp
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            IdentityModelEventSource.ShowPII = true;

            HttpClient httpClient = new HttpClient();

            var discoveryDocumentResponse = await httpClient.GetDiscoveryDocumentAsync("https://localhost:7233");

            if (discoveryDocumentResponse.IsError)
                Console.WriteLine(discoveryDocumentResponse.Error);

            var response = await httpClient.RequestDeviceAuthorizationAsync(new DeviceAuthorizationRequest
            {
                ClientId = "client1",
                Scope = "openid offline_access profile email",
                RequestUri = new Uri(discoveryDocumentResponse.DeviceAuthorizationEndpoint),
            });

            if (response.IsError)
            {
                Console.WriteLine(response.Error);
                return;
            }

            Console.WriteLine(response.Raw);

            while (true)
            {
                var tokenResponse = await httpClient.RequestDeviceTokenAsync(new DeviceTokenRequest
                {
                    Address = discoveryDocumentResponse.TokenEndpoint,
                    ClientId = "client1",
                    DeviceCode = response.DeviceCode,
                });

                Console.WriteLine(tokenResponse.Raw);

                if (tokenResponse.IsError)
                {
                    await Task.Delay(TimeSpan.FromSeconds(response.Interval));
                }
                else
                {
                    break;
                }
            }
             
        }
    }
}
