using EasyIdentity.Models;
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

    public EasyIdentityBuilder DisableAuthorizationCodeFlow()
    {
        // TODO
        return this;
    }

    public EasyIdentityBuilder DisableDeviceCodeFlowFlow()
    {
        // TODO
        return this;
    }

    public EasyIdentityBuilder DisableClientCredentialsFlow()
    {
        // TODO
        return this;
    }

    public EasyIdentityBuilder DisablePasswordFlow()
    {
        // TODO
        return this;
    }

    public EasyIdentityBuilder DisableRefreshTokenFlow()
    {
        // TODO
        return this;
    }

    public EasyIdentityBuilder DisableImplicitFlow()
    {
        // TODO
        return this;
    }

    public EasyIdentityBuilder DisableCibaFlow()
    {
        // TODO
        return this;
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

    public EasyIdentityBuilder AddUserService<TUserService>() where TUserService : class, IUserService
    {
        Services.AddScoped<IUserService, TUserService>();
        return this;
    }

    public EasyIdentityBuilder AddDevelopmentECDsaCredentialsStore()
    {
        Services.AddTransient<ICredentialsService, DevelopmentECDsaCredentialsService>();
        return this;
    }

    public EasyIdentityBuilder AddDevelopmentRSACredentialsStore()
    {
        Services.AddTransient<ICredentialsService, DevelopmentRSACredentialsService>();
        return this;
    }

    public EasyIdentityBuilder AddCredentialsStore<TStore>() where TStore : class, ICredentialsService
    {
        Services.AddTransient<ICredentialsService, TStore>();
        return this;
    }

    public EasyIdentityBuilder AddAuthorizationCodeService<TAuthorizationCode, TAuthorizationCodeStoreImplementation>()
        where TAuthorizationCodeStoreImplementation : class, IAuthorizationCodeStore<TAuthorizationCode>
        where TAuthorizationCode : class, new()
    {
        Services.AddScoped<IAuthorizationCodeStore<TAuthorizationCode>, TAuthorizationCodeStoreImplementation>();
        Services.AddScoped<IAuthorizationCodeFlowManager, AuthorizationCodeFlowManager<TAuthorizationCode>>();

        return this;
    }

    public EasyIdentityBuilder AddDeviceCodeService<TDeviceCode, TDeviceCodeStoreImplementation>()
        where TDeviceCodeStoreImplementation : class, IDeviceCodeStore<TDeviceCode>
        where TDeviceCode : class, new()
    {
        Services.AddScoped<IDeviceCodeStore<TDeviceCode>, TDeviceCodeStoreImplementation>();
        Services.AddScoped<IDeviceCodeFlowManager, DeviceCodeFlowManager<TDeviceCode>>();

        return this;
    }

    public EasyIdentityBuilder AddTokenService<TToken, TTokenStoreImplementation>()
        where TTokenStoreImplementation : class, ITokenStore<TToken>
        where TToken : class, new()
    {
        Services.AddScoped<ITokenStore<TToken>, TTokenStoreImplementation>();
        Services.AddScoped<ITokenManager, TokenManager<TToken>>();

        return this;
    }

}
