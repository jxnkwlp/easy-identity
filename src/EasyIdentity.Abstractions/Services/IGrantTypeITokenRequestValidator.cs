using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services
{
    public interface IGrantTypeITokenRequestValidator
    {
        string GrantType { get; }

        Task<RequestValidationResult> ValidateAsync(RequestData requestData);
    }
}
