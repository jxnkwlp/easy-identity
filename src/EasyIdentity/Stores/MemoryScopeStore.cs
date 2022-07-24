using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Stores;

public class MemoryScopeStore : IScopeStore
{
    public static List<Scope> List { get; } = new();

    public static void AddScope(Scope scope)
    {
        List.Add(scope);
    }

    public Task<Scope> FindByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var find = List.FirstOrDefault(x => x.Name == name);
        return Task.FromResult(find);
    }
}
