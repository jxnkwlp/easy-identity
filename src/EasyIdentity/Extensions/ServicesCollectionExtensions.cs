using EasyIdentity.Endpoints;
using EasyIdentity.Services;
using EasyIdentity.Stores;
using Microsoft.Extensions.DependencyInjection;

namespace EasyIdentity.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static EasyIdentityOptionsBuilder AddEasyIdentity(this IServiceCollection services)
        {
            var builder = new EasyIdentityOptionsBuilder(services, new EasyIdentityOptions());

            services.AddHttpContextAccessor();

            services.AddSingleton((_) => builder);
            services.AddOptions<EasyIdentityOptions>();

            services.AddEasyIdentityEndpointHandler<DiscoveryEndpointHandler>();
            services.AddEasyIdentityEndpointHandler<JwksEndpointHandler>();
            services.AddEasyIdentityEndpointHandler<AuthorizationEndpointHandler>();
            services.AddEasyIdentityEndpointHandler<TokenEndpointHandler>();
            services.AddEasyIdentityEndpointHandler<DeviceCodeEndpointHandler>();

            services.AddTransient<IClientStore, MemoryClientStore>();

            services.AddTransient<IRequestParamReader, RequestParamReader>();
            services.AddTransient<ITokenRequestValidator, TokenRequestValidator>();
            services.AddTransient<IResponseWriter, ResponseWriter>();

            services.AddTransient<IClientCredentialsIdentityCreationService, ClientCredentialsIdentityCreationService>();

            services.AddTransient<IGrantTypeTokenRequestValidator, ClientCredentialsTokenRequestValidator>();
            services.AddTransient<IGrantTypeTokenRequestValidator, PasswordTokenRequestValidator>();
            services.AddTransient<IGrantTypeTokenRequestValidator, AuthorizationCodeTokenRequestValidator>();
            services.AddTransient<IGrantTypeTokenRequestValidator, ImplicitTokenRequestValidator>();

            services.AddTransient<IGrantTypeHandler, ClientCredentialsGrantTypeHandler>();
            services.AddTransient<IGrantTypeHandler, PasswordGrantTypeHandler>();
            services.AddTransient<IGrantTypeHandler, AuthorizationCodeGrantTypeHandler>();
            services.AddTransient<IGrantTypeHandler, ImplicitGrantTypeHandler>();

            services.AddTransient<IAuthorizationCodeManager, AuthorizationCodeManager>();
            services.AddTransient<IAuthorizationRequestValidator, AuthorizationRequestValidator>();
            services.AddTransient<IAuthorizationCodeCreationService, AuthorizationCodeCreationService>();
            services.AddTransient<IAuthorizationCodeStoreService, AuthorizationCodeStoreService>();

            services.AddTransient<IDeviceCodeManager, DeviceCodeService>();
            services.AddTransient<IDeviceCodeRequestValidator, DeviceCodeRequestValidator>();
            services.AddTransient<IDeviceCodeCodeCreationService, DeviceCodeCodeCreationService>();
            services.AddTransient<IDeviceCodeStoreService, DeviceCodeStoreService>();

            services.AddTransient<ITokenManager, TokenManager>();
            services.AddTransient<ITokenCreationService, JwtTokenCreationService>();

            services.AddTransient<IAuthorizationInteractionService, AuthorizationInteractionService>();

            services.AddTransient<IJsonSerializer, DefaultJsonSerializer>();

            services.AddTransient<IRedirectUrlValidator, RedirectUrlValidator>();

            services.AddSingleton<EasyIdentityOptions>((_) => builder.Options);

            return builder;
        }

        public static IServiceCollection AddEasyIdentityEndpointHandler<TEndpointHandler>(this IServiceCollection services) where TEndpointHandler : class, IEndpointHandler
        {
            return services.AddTransient<IEndpointHandler, TEndpointHandler>();
        }

        public static IServiceCollection AddEasyIdentityUserProfileService<TProfileService>(this IServiceCollection services) where TProfileService : class, IUserService
        {
            return services.AddScoped<IUserService, TProfileService>();
        }

    }
}
