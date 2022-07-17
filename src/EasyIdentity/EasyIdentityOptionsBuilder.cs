using System;
using System.Security.Cryptography;
using EasyIdentity.Models;
using EasyIdentity.Services;
using EasyIdentity.Stores;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace EasyIdentity;

public class EasyIdentityOptionsBuilder
{
    public EasyIdentityOptions Options { get; protected set; }

    private readonly IServiceCollection _services;

    public EasyIdentityOptionsBuilder(IServiceCollection services, EasyIdentityOptions options)
    {
        _services = services;
        Options = options;
    }

    public EasyIdentityOptionsBuilder AddClient(Client client)
    {
        MemoryClientStore.Clients.Add(client);

        return this;
    }

    public EasyIdentityOptionsBuilder AddStandardScopes()
    {
        MemoryScopeStore.List.Add(new Scope(StandardScopes.Email, ""));
        MemoryScopeStore.List.Add(new Scope(StandardScopes.Profile, ""));
        MemoryScopeStore.List.Add(new Scope(StandardScopes.OpenId, ""));
        MemoryScopeStore.List.Add(new Scope(StandardScopes.OfflineAccess, ""));
        MemoryScopeStore.List.Add(new Scope(StandardScopes.Phone, ""));
        MemoryScopeStore.List.Add(new Scope(StandardScopes.Address, ""));

        return this;
    }

    public EasyIdentityOptionsBuilder AddDevelopmentSigningCredentials()
    {
        var rsaSecurityKey = new RsaSecurityKey(RSA.Create(2048)) { KeyId = Guid.NewGuid().ToString("N") };
        Options.SigningCredentials.Add(new SigningCredentials(rsaSecurityKey, SecurityAlgorithms.RsaSha256));

        //var ecdSecurityKey = new ECDsaSecurityKey(ECDsa.Create(ECCurve.NamedCurves.nistP256)) { KeyId = Guid.NewGuid().ToString("N") };
        //Options.SigningCredentials.Add(new SigningCredentials(ecdSecurityKey, SecurityAlgorithms.EcdsaSha256));

        return this;
    }

    public EasyIdentityOptionsBuilder AddUserProfileService<TProfileService>() where TProfileService : class, IUserService
    {
        _services.AddScoped<IUserService, TProfileService>();
        return this;
    }

}
