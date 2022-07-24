using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

/// <summary>
///  Device flow code manager class
/// </summary>
public interface IDeviceCodeFlowManager
{
    Task<DeviceCodeData> RequestAsync(RequestData requestData, RequestValidationResult validationResult, CancellationToken cancellationToken = default);

    Task<bool> ValidateDeviceCodeAsync(string deviceCode, Client client, CancellationToken cancellationToken = default);

    Task<string> FindDeviceCodeAsync(string userCode, CancellationToken cancellationToken = default);

    Task<string> FindClientIdAsync(string deviceCode, CancellationToken cancellationToken = default);

    Task<string> FindSubjectAsync(string deviceCode, Client client, CancellationToken cancellationToken = default);

    Task<bool> CheckGrantedAsync(string deviceCode, Client client, CancellationToken cancellationToken = default);

    Task GrantAsync(string deviceCode, Client client, ClaimsPrincipal principal, CancellationToken cancellationToken = default);

    Task RejectAsync(string deviceCode, Client client, ClaimsPrincipal principal, CancellationToken cancellationToken = default);

}
