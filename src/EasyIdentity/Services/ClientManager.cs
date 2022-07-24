using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;
using EasyIdentity.Stores;

namespace EasyIdentity.Services;

public class ClientManager : IClientManager
{
    private readonly IClientStore _clientStore;

    public ClientManager(IClientStore clientStore)
    {
        _clientStore = clientStore;
    }

    public async Task<Client> FindByClientIdAsync(string clientId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(clientId))
        {
            throw new ArgumentException($"'{nameof(clientId)}' cannot be null or whitespace.", nameof(clientId));
        }

        return await _clientStore.FindByClientIdAsync(clientId, cancellationToken);
    }

    public Task<List<Client>> GetClientsAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
