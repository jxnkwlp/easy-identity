using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public interface IAuthorizationCodeFlowManager
{
    Task<string> CreateCodeAsync(Client client, string[] scopes, string subject, ClaimsPrincipal claimsPrincipal, RequestData requestData, CancellationToken cancellationToken = default);

    Task<string> GetSubjectAsync(Client client, string[] scopes, string code, CancellationToken cancellationToken = default);

    Task<AuthorizationCodeValidationResult> ValidationAsync(Client client, string code, RequestData requestData, CancellationToken cancellationToken = default);
}
