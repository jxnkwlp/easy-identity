using System;
using EasyIdentity.Endpoints;
using EasyIdentity.Services;
using EasyIdentity.Stores;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EasyIdentity.Extensions;

public static class ServicesCollectionExtensions
{
    public static EasyIdentityBuilder AddEasyIdentity(this IServiceCollection services, Action<EasyIdentityOptions> optionSetup)
    {
        if (optionSetup is null)
        {
            throw new ArgumentNullException(nameof(optionSetup));
        }

        // depends on
        services
            .AddOptions()
            .AddLogging()
            .AddHttpContextAccessor();

        var builder = new EasyIdentityBuilder(services);

        services.AddSingleton((_) => builder);

        services.Configure(optionSetup);

        services
            .AddOptions<EasyIdentityOptions>()
#if NET5_0_OR_GREATER
            .BindConfiguration(EasyIdentityOptions.EasyIdentity)
#else
            .Configure<IConfiguration>((opt, configure) =>
            {
                configure.Bind(EasyIdentityOptions.EasyIdentity, opt);
            })
#endif
            .Configure(optionSetup)
            ;

        // EndpointHandler
        services.AddEasyIdentityEndpointHandler<DiscoveryEndpointHandler>();
        services.AddEasyIdentityEndpointHandler<JwksEndpointHandler>();
        services.AddEasyIdentityEndpointHandler<AuthorizationEndpointHandler>();
        services.AddEasyIdentityEndpointHandler<TokenEndpointHandler>();
        services.AddEasyIdentityEndpointHandler<DeviceCodeEndpointHandler>();

        // Store
        services.AddSingleton<IClientStore, MemoryClientStore>();
        services.AddScoped<ISigningCredentialsStore, EmptySigningCredentialsStore>();

        // request/response
        services.AddScoped<IRequestParamReader, RequestParamReader>();
        services.AddScoped<ITokenRequestValidator, TokenRequestValidator>();
        services.AddScoped<IResponseWriter, ResponseWriter>();

        // Token
        services.AddScoped<ITokenManager, TokenManager>();
        services.AddScoped<ITokenCreationService, JwtTokenCreationService>();

        // common service
        services.AddScoped<IRedirectUrlValidator, RedirectUrlValidator>();
        services.AddSingleton<IJsonSerializer, DefaultJsonSerializer>();

        // 
        services.AddScoped<IAuthorizationInteractionService, AuthorizationInteractionService>();

        // GrantTypeTokenRequestValidator
        services.AddScoped<IGrantTypeTokenRequestValidator, ClientCredentialsTokenRequestValidator>();
        services.AddScoped<IGrantTypeTokenRequestValidator, PasswordTokenRequestValidator>();
        services.AddScoped<IGrantTypeTokenRequestValidator, AuthorizationCodeTokenRequestValidator>();
        services.AddScoped<IGrantTypeTokenRequestValidator, ImplicitTokenRequestValidator>();
        services.AddScoped<IGrantTypeTokenRequestValidator, DeviceCodeTokenRequestValidator>();

        // GrantTypeHandler
        services.AddScoped<IGrantTypeHandler, ClientCredentialsGrantTypeHandler>();
        services.AddScoped<IGrantTypeHandler, PasswordGrantTypeHandler>();
        services.AddScoped<IGrantTypeHandler, AuthorizationCodeGrantTypeHandler>();
        services.AddScoped<IGrantTypeHandler, ImplicitGrantTypeHandler>();
        services.AddScoped<IGrantTypeHandler, DeviceCodeGrantTypeHandler>();

        // ClientCredentials
        services.AddScoped<IClientCredentialsIdentityCreationService, ClientCredentialsIdentityCreationService>();

        // AuthorizationCode
        services.AddScoped<IAuthorizationCodeManager, AuthorizationCodeManager>();
        services.AddScoped<IAuthorizationRequestValidator, AuthorizationRequestValidator>();
        services.AddScoped<IAuthorizationCodeCreationService, AuthorizationCodeCreationService>();
        services.AddScoped<IAuthorizationCodeStoreService, AuthorizationCodeStoreService>();

        // DeviceCode
        services.AddScoped<IDeviceCodeManager, DeviceCodeManager>();
        services.AddScoped<IDeviceCodeRequestValidator, DeviceCodeRequestValidator>();
        services.AddScoped<IDeviceCodeCodeCreationService, DeviceCodeCodeCreationService>();
        services.AddScoped<IDeviceCodeStoreService, DeviceCodeStoreService>();

        return builder;
    }

    public static IServiceCollection AddEasyIdentityEndpointHandler<TEndpointHandler>(this IServiceCollection services) where TEndpointHandler : class, IEndpointHandler
    {
        return services.AddScoped<IEndpointHandler, TEndpointHandler>();
    }

    public static IServiceCollection AddEasyIdentityUserProfileService<TProfileService>(this IServiceCollection services) where TProfileService : class, IUserService
    {
        return services.AddScoped<IUserService, TProfileService>();
    }

}
