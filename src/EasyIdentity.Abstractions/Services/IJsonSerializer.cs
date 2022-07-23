using System.Threading.Tasks;

namespace EasyIdentity.Services;

public interface IJsonSerializer
{
    Task<string> SerializeAsync<T>(T data);
}
