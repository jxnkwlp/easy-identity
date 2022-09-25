using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EasyIdentity.Extensions;
using EasyIdentity.Models;
using EasyIdentity.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace EasyIdentity.Endpoints;

/// <summary>
///  OIDC endpoint - userinfo
/// </summary>
public class UserinfoEndpointHanlder : IEndpointHandler
{
    public string Path => EndpointProtocolRoutePaths.UserInfo;

    public string[] Methods => new string[] { HttpMethods.Get };

    private readonly ILogger<UserinfoEndpointHanlder> _logger;
    private readonly EasyIdentityOptions _easyIdentityOptions;
    private readonly IUserService _userService;
    private readonly IResponseWriter _responseWriter;
    private readonly IRequestParamReader _requestParamReader;

    public UserinfoEndpointHanlder(ILogger<UserinfoEndpointHanlder> logger, EasyIdentityOptions easyIdentityOptions, IUserService userService, IResponseWriter responseWriter, IRequestParamReader requestParamReader)
    {
        _logger = logger;
        _easyIdentityOptions = easyIdentityOptions;
        _userService = userService;
        _responseWriter = responseWriter;
        _requestParamReader = requestParamReader;
    }

    public async Task HandleAsync(HttpContext context)
    {
        var requestData = await _requestParamReader.ReadAsync();

        var authenticateResult = await context.AuthenticateAsync(_easyIdentityOptions.AuthenticationScheme);

        if (authenticateResult.Succeeded == false)
        {
            await _responseWriter.WriteAsync(new ResponseDescriptor(requestData, "invalid_request", authenticateResult.Failure?.Message));
            return;
        }

        var claimsPrincipal = authenticateResult.Principal;
        var subject = claimsPrincipal.GetSubject();

        var userProfile = await _userService.GetProfileAsync(new UserProfileRequest(null, subject, requestData));

        var claims = userProfile.Principal.Claims.ToList();
        if (userProfile.Principal.HasClaim(x => x.Type == ClaimTypes.NameIdentifier))
        {
            claims.Add(new Claim(StandardClaimTypes.Sub, userProfile.Principal.FindFirstValue(ClaimTypes.NameIdentifier)));
        }

        // TODO 
        await _responseWriter.WriteAsync(new ResponseDescriptor(requestData, claims.ToDictionary(x => x.Type, x => (object)x.Value)));
    }
}
