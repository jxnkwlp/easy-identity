using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Stores;

public interface IClientStore
{
    Task<Client> FindByClientIdAsync(string clientId, CancellationToken cancellationToken = default);
}
