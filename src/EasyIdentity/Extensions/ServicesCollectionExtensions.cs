using System;
using EasyIdentity.Endpoints;
using EasyIdentity.Models;
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

        var builder = new EasyIdentityBuilder(services);

        // depends on
        services
            .AddOptions()
            .AddLogging()
            .AddHttpContextAccessor();

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

        // request/response
        services.AddScoped<IRequestParamReader, RequestParamReader>();
        services.AddScoped<ITokenRequestValidator, TokenRequestValidator>();
        services.AddScoped<IResponseWriter, ResponseWriter>();


        services.AddScoped<IClientAuthenticationService, ClientAuthenticationService>();

        // Token
        services.AddScoped<ITokenCreationService, JwtTokenCreationService>();
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();

        // common service
        services.AddScoped<IRedirectUrlValidator, RedirectUrlValidator>();
        services.AddSingleton<IJsonSerializer, DefaultJsonSerializer>();
        services.AddSingleton<ICryptographyHelper, CryptographyHelper>();

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
        services.AddScoped<IAuthorizationRequestValidator, AuthorizationRequestValidator>();
        services.AddScoped<IAuthorizationCodeCreationService, AuthorizationCodeCreationService>();
        // store 
        builder.AddAuthorizationCodeService<EasyIdentityAuthorizationCode, AuthorizationCodeStore>();

        // DeviceCode 
        services.AddScoped<IDeviceCodeRequestValidator, DeviceCodeRequestValidator>();
        services.AddScoped<IDeviceCodeCodeCreationService, DeviceCodeCodeCreationService>();
        // store
        builder.AddDeviceCodeService<EasyIdentityDeviceCode, DeviceCodeStore>();

        //  
        services.AddScoped<IScopeManager, ScopeManager>();
        services.AddScoped<IScopeStore, MemoryScopeStore>();
        //
        services.AddScoped<IClientManager, ClientManager>();
        services.AddSingleton<IClientStore, MemoryClientStore>();
        // 
        services.AddScoped<ICredentialsService, EmptyCredentialsService>();

        // token 
        builder.AddTokenService<EasyIdentityToken, TokenStore>();

        // 
        services.AddSingleton((_) => builder);

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
