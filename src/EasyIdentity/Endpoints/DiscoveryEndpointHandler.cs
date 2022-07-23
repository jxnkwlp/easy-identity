using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Endpoints;

public class DiscoveryEndpointHandler : IEndpointHandler
{
    public string Path => EndpointProtocolRoutePaths.DiscoveryConfiguration;

    public string[] Methods => new string[1] { HttpMethods.Get };

    public async Task HandleAsync(HttpContext context)
    {
        var request = context.Request;

        var issuer = $"{request.Scheme}://{request.Host}{request.PathBase}";

        var response = new
        {
            issuer = issuer,
            jwks_uri = $"{issuer}/.well-known/openid-configuration/jwks",
            authorization_endpoint = $"{issuer}/connect/authorize",
            token_endpoint = $"{issuer}/connect/token",
            userinfo_endpoint = $"{issuer}/connect/userinfo",
            end_session_endpoint = $"{issuer}/connect/endsession",
            check_session_iframe = $"{issuer}/connect/checksession",
            revocation_endpoint = $"{issuer}/connect/revocation",
            introspection_endpoint = $"{issuer}/connect/introspect",
            device_authorization_endpoint = $"{issuer}/connect/device",
            frontchannel_logout_supported = true,
            frontchannel_logout_session_supported = true,
            backchannel_logout_supported = true,
            scopes_supported = new[] { "openid", "profile", "email", "api1", "api2.read_only" },
            response_types_supported = new[] { "code", "id_token", "code id_token", "code token", "id_token token", "code id_token token" },
            response_modes_supported = new[] { "query", "fragment", "form_post" },
            grant_types_supported = new[] { "authorization_code", "implicit" },
            subject_types_supported = new[] { "public" },
            id_token_signing_alg_values_supported = new[] { "RS256" },
            code_challenge_methods_supported = new[] { "plain", "S256" },
            token_endpoint_auth_methods_supported = new[] { "client_secret_post", "client_secret_basic" },
            token_endpoint_auth_signing_alg_values_supported = new[] { "RS256" },
            display_values_supported = new[] { "page", "popup", "touch" }
        };

        await context.Response.WriteAsJsonAsync(response);
    }
}
