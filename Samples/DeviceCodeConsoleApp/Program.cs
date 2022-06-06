using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace DeviceCodeConsoleApp
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            HttpClient httpClient = new HttpClient() { BaseAddress = new Uri("https://localhost:7139/") };

            var discoveryDocumentResponse = await httpClient.GetDiscoveryDocumentAsync();

            if (discoveryDocumentResponse.IsError)
                Console.WriteLine(discoveryDocumentResponse.Error);

            var response = await httpClient.RequestDeviceAuthorizationAsync(new DeviceAuthorizationRequest
            {
                ClientId = "client1",
                ClientSecret = "1234567890",
                RequestUri = new Uri(discoveryDocumentResponse.DeviceAuthorizationEndpoint),
            });

            if (response.IsError)
                Console.WriteLine(response.Error);

            Console.WriteLine(response.Raw);
        }
    }
}
