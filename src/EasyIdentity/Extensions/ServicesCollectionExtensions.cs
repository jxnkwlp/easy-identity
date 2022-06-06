using System;
using EasyIdentity.Endpoints;
using EasyIdentity.Services;
using EasyIdentity.Stores;
using Microsoft.Extensions.DependencyInjection;

namespace EasyIdentity.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static EasyIdentityOptionsBuilder AddEasyIdentity(this IServiceCollection services, Action<EasyIdentityOptionsBuilder> configure = null)
        {
            var builder = new EasyIdentityOptionsBuilder(services, new EasyIdentityOptions());


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

            services.AddTransient<IGrantTypeITokenRequestValidator, ClientCredentialsTokenRequestValidator>();
            services.AddTransient<IGrantTypeITokenRequestValidator, PasswordTokenRequestValidator>();
            services.AddTransient<IGrantTypeITokenRequestValidator, AuthorizationCodeTokenRequestValidator>();
            services.AddTransient<IGrantTypeITokenRequestValidator, ImplicitTokenRequestValidator>();

            services.AddTransient<IGrantTypeHandler, ClientCredentialsGrantTypeHandler>();
            services.AddTransient<IGrantTypeHandler, PasswordGrantTypeHandler>();
            services.AddTransient<IGrantTypeHandler, AuthorizationCodeGrantTypeHandler>();
            services.AddTransient<IGrantTypeHandler, ImplicitGrantTypeHandler>();

            services.AddTransient<IAuthorizationRequestValidator, AuthorizationRequestValidator>();
            services.AddTransient<IAuthorizationCodeCreationService, AuthorizationCodeCreationService>();
            services.AddTransient<IAuthorizationCodeStoreService, AuthorizationCodeStoreService>();

            services.AddTransient<ITokenResponseWriter, TokenResponseWriter>();
            services.AddTransient<ITokenCreationService, JwtTokenGeneratorService>();

            services.AddTransient<IJsonSerializer, DefaultJsonSerializer>();

            configure?.Invoke(builder);

            services.AddSingleton<EasyIdentityOptions>((_) => builder.Options);

            return builder;
        }

        public static IServiceCollection AddEasyIdentityEndpointHandler<TEndpointHandler>(this IServiceCollection services) where TEndpointHandler : class, IEndpointHandler
        {
            return services.AddTransient<IEndpointHandler, TEndpointHandler>();
        }

        public static IServiceCollection AddEasyIdentityUserProfileService<TProfileService>(this IServiceCollection services) where TProfileService : class, IUserProfileService
        {
            return services.AddScoped<IUserProfileService, TProfileService>();
        }

    }
}
