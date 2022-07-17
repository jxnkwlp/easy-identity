using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services
{
    public interface ITokenRequestValidator
    {
        Task<RequestValidationResult> ValidateAsync(string grantType, RequestData data);
    }
}
