using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public interface IAuthorizationCodeFlowManager
{
    Task<string> GetSubjectAsync(string code, Client client, CancellationToken cancellationToken = default);

    Task<string> CreateCodeAsync(Client client, ClaimsPrincipal claimsPrincipal, RequestData requestData, CancellationToken cancellationToken = default);

    Task<AuthorizationCodeValidationResult> ValidationAsync(string code, Client client, RequestData requestData, CancellationToken cancellationToken = default);
}
