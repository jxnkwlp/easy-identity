using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasyIdentity.Services;

public class DefaultJsonSerializer : IJsonSerializer
{
    private static readonly JsonSerializerOptions _cache = new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true,
    };

    protected JsonSerializerOptions Create()
    {
        return _cache;
    }

    public async Task<string> SerializeAsync<T>(T data)
    {
        using (var ms = new MemoryStream())
        {
            await JsonSerializer.SerializeAsync(ms, data, typeof(T), Create());
            ms.Position = 0;
            using (var sr = new StreamReader(ms))
            {
                return await sr.ReadToEndAsync();
            }
        }
    }
}
