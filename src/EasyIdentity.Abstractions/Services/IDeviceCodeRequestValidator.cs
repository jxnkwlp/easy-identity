using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services
{
    public interface IDeviceCodeRequestValidator
    {
        Task<RequestValidationResult> ValidateAsync(RequestData data);
    }
}
