using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public interface IAuthorizationRequestValidator
{
    Task<RequestValidationResult> ValidateAsync(RequestData data);
}
