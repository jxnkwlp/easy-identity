using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Stores;

public class MemoryClientStore : IClientStore
{
    public static List<Client> Clients { get; } = new();

    public Task<Client> FindByClientIdAsync(string clientId, CancellationToken cancellationToken = default)
    {
        var find = Clients.FirstOrDefault(x => x.ClientId == clientId);

        return Task.FromResult(find);
    }
}
