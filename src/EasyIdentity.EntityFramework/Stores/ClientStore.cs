using System;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;
using EasyIdentity.Stores;

namespace EasyIdentity.EntityFramework.Stores;

public class ClientStore : IClientStore
{
    public Task<Client> FindByClientIdAsync(string clientId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
