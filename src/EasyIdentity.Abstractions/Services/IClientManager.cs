using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public interface IClientManager
{
    Task<List<Client>> GetClientsAsync(CancellationToken cancellationToken = default);
    Task<Client> FindByClientIdAsync(string clientId, CancellationToken cancellationToken = default);
}
