﻿using EasyIdentity.Models;
using EasyIdentity.Services;
using EasyIdentity.Stores;
using Microsoft.Extensions.DependencyInjection;

namespace EasyIdentity;

public class EasyIdentityBuilder
{
    protected IServiceCollection Services { get; }

    public EasyIdentityBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public EasyIdentityBuilder AddClient(Client client)
    {
        MemoryClientStore.Clients.Add(client);

        return this;
    }

    public EasyIdentityBuilder AddStandardScopes()
    {
        MemoryScopeStore.List.Add(new Scope(StandardScopes.Email, ""));
        MemoryScopeStore.List.Add(new Scope(StandardScopes.Profile, ""));
        MemoryScopeStore.List.Add(new Scope(StandardScopes.OpenId, ""));
        MemoryScopeStore.List.Add(new Scope(StandardScopes.OfflineAccess, ""));
        MemoryScopeStore.List.Add(new Scope(StandardScopes.Phone, ""));
        MemoryScopeStore.List.Add(new Scope(StandardScopes.Address, ""));

        return this;
    }

    public EasyIdentityBuilder AddUserProfileService<TProfileService>() where TProfileService : class, IUserService
    {
        Services.AddScoped<IUserService, TProfileService>();
        return this;
    }

    public EasyIdentityBuilder AddDevelopmentECDsaSigningCredentialsStore()
    {
        Services.AddTransient<ISigningCredentialsStore, DevelopmentECDsaSigningCredentialsStore>();
        return this;
    }

    public EasyIdentityBuilder AddDevelopmentRSASigningCredentialsStore()
    {
        Services.AddTransient<ISigningCredentialsStore, DevelopmentRSASigningCredentialsStore>();
        return this;
    }
}
