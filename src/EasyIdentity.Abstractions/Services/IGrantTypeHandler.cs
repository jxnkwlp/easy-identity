using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public interface IGrantTypeHandler
{
    string GrantType { get; }

    Task<GrantTypeExecutionResult> ExecuteAsync(GrantTypeExecutionRequest request, CancellationToken cancellationToken = default);
}
