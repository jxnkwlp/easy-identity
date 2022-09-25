using System.Threading;
using System.Threading.Tasks;

namespace EasyIdentity.Services;

public interface IDeviceCodeFlowThrottlingService
{
    Task<bool> ShouldSlowDownAsync(string deviceCode, CancellationToken cancellationToken = default);
}
