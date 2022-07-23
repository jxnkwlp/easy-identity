using System;
using System.Security.Claims;
using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.Extensions.Options;

namespace EasyIdentity.Services;

public class AuthorizationCodeManager : IAuthorizationCodeManager
{
    private readonly EasyIdentityOptions _options;
    private readonly IAuthorizationCodeCreationService _authorizationCodeCreationService;
    private readonly IAuthorizationCodeStoreService _authorizationCodeStoreService;

    public AuthorizationCodeManager(IOptions<EasyIdentityOptions> options, IAuthorizationCodeCreationService authorizationCodeCreationService, IAuthorizationCodeStoreService authorizationCodeStoreService)
    {
        _options = options.Value;
        _authorizationCodeCreationService = authorizationCodeCreationService;
        _authorizationCodeStoreService = authorizationCodeStoreService;
    }

    public async Task<string> CreateCodeAsync(Client client, ClaimsPrincipal claimsPrincipal)
    {
        var code = await _authorizationCodeCreationService.CreateAsync(client, claimsPrincipal);

        await _authorizationCodeStoreService.CreateAsync(claimsPrincipal, code, DateTime.UtcNow.Add(_options.DefaultAuthorizationCodeLifetime));

        return code;
    }

    public async Task<string> GetSubjectAsync(string code)
    {
        return await _authorizationCodeStoreService.GetSubjectAsync(code);
    }

}
