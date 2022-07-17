using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services
{
    public interface IDeviceCodeCodeCreationService
    {
        Task<string> CreateDeviceCodeAsync(Client client);

        Task<string> CreateUserCodeAsync(Client client);

    }
}
