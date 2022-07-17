using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Http
{
    internal static class HttpResponseExtension
    {

#if NETCOREAPP3_1

        public static async Task WriteAsJsonAsync<T>(this HttpResponse response, T data, JsonSerializerOptions jsonSerializerOptions, CancellationToken cancellationToken = default)
        {
            var json = JsonSerializer.Serialize(data, jsonSerializerOptions);
            response.ContentType = "application/json";
            await response.WriteAsync(json, cancellationToken);
        }

        public static async Task WriteAsJsonAsync<T>(this HttpResponse response, T data, CancellationToken cancellationToken = default)
        {
            await WriteAsJsonAsync(response, data, new JsonSerializerOptions(JsonSerializerDefaults.Web), cancellationToken);
        }

#endif

    }
}
