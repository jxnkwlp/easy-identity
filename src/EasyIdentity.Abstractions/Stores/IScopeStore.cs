﻿using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Stores;

public interface IScopeStore
{
    Task<Scope> FindByNameAsync(string name, CancellationToken cancellationToken = default);
}
