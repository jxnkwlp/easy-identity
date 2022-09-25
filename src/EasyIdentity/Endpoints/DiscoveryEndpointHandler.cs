using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyIdentity.Extensions;
using EasyIdentity.Models;
using EasyIdentity.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EasyIdentity.Endpoints;

public class DiscoveryEndpointHandler : IEndpointHandler
{
    public string Path => EndpointProtocolRoutePaths.DiscoveryConfiguration;

    public string[] Methods => new string[1] { HttpMethods.Get };

    private readonly ILogger<DiscoveryEndpointHandler> _logger;
    private readonly EasyIdentityOptions _identityOptions;
    private readonly IEnumerable<IGrantTypeHandler> _grantTypeHandlers;

    public DiscoveryEndpointHandler(ILogger<DiscoveryEndpointHandler> logger, IOptions<EasyIdentityOptions> identityOptions, IEnumerable<IGrantTypeHandler> grantTypeHandlers)
    {
        _logger = logger;
        _identityOptions = identityOptions.Value;
        _grantTypeHandlers = grantTypeHandlers;
    }

    public async Task HandleAsync(HttpContext context)
   {
        _logger.LogInformation("Request discovery infomation.");

        var request = context.Request;

        var issuer = _identityOptions.Issuer?.EnsureEndsWith('/') ?? $"{request.Scheme}://{request.Host}{request.PathBase}/";

        var grantTypes = _grantTypeHandlers.Select(selector: x => x.GrantType).Distinct().ToArray();
        var scopes = new List<string> { StandardScopes.OpenId, StandardScopes.Profile, StandardScopes.Email, StandardScopes.Address, StandardScopes.Phone, StandardScopes.OfflineAccess };


        var response = new
        {
            issuer = issuer,
            jwks_uri = $"{issuer}{EndpointProtocolRoutePaths.DiscoveryWebKeys}",
            authorization_endpoint = $"{issuer}{EndpointProtocolRoutePaths.Authorize}",
            token_endpoint = $"{issuer}{EndpointProtocolRoutePaths.Token}",
            userinfo_endpoint = $"{issuer}{EndpointProtocolRoutePaths.UserInfo}",
            end_session_endpoint = $"{issuer}connect/endsession",
            check_session_iframe = $"{issuer}connect/checksession",
            revocation_endpoint = $"{issuer}connect/revocation",
            introspection_endpoint = $"{issuer}connect/introspect",
            device_authorization_endpoint = $"{issuer}{EndpointProtocolRoutePaths.DeviceCode}",
            frontchannel_logout_supported = true,
            frontchannel_logout_session_supported = true,
            backchannel_logout_supported = true,
            scopes_supported = scopes,
            response_types_supported = new[] { "code", "token", "code token", "code id_token", "code id_token token", "id_token", "id_token token" },
            response_modes_supported = new[] { "query", "fragment", "form_post" },
            grant_types_supported = grantTypes,
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
