using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public interface IScopeManager
{
    Task<Scope> FindByNameAsync(string name, CancellationToken cancellationToken = default);
}
