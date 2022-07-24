using System;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;
using EasyIdentity.Stores;

namespace EasyIdentity.Services;

public class ScopeManager : IScopeManager
{
    private readonly IScopeStore _scopeStore;

    public ScopeManager(IScopeStore scopeStore)
    {
        _scopeStore = scopeStore;
    }

    public async Task<Scope> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
        }

        return await _scopeStore.FindByNameAsync(name, cancellationToken);
    }
}
