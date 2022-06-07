using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services
{
    public interface IAuthorizationInteractionService
    {
        Task<DeviceCodeAuthorizationResult> DeviceCodeAuthorizationAsync(string userCode);
    }
}
