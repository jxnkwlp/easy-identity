using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public interface IGrantTypeTokenRequestValidator
{
    string GrantType { get; }

    Task<RequestValidationResult> ValidateAsync(RequestData data);
}
