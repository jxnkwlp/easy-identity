using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services
{
    public interface IDeviceCodeCodeCreationService
    {
        Task<string> CreateCodeAsync(Client client);
        Task<string> CreateUserCodeAsync(Client client);


    }
}
