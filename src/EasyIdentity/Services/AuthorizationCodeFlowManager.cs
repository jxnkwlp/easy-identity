using System;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;
using EasyIdentity.Stores;
using Microsoft.Extensions.Options;

namespace EasyIdentity.Services;

/// <summary>
///  Internal class
/// </summary>
public class AuthorizationCodeFlowManager<TAuthorizationCode> : IAuthorizationCodeFlowManager where TAuthorizationCode : class
{
    private readonly EasyIdentityOptions _options;
    private readonly ICryptographyHelper _cryptographyHelper;
    private readonly IAuthorizationCodeCreationService _authorizationCodeCreationService;
    private readonly IAuthorizationCodeStore<TAuthorizationCode> _authorizationCodeStoreService;

    public AuthorizationCodeFlowManager(IOptions<EasyIdentityOptions> options, ICryptographyHelper cryptographyHelper, IAuthorizationCodeCreationService authorizationCodeCreationService, IAuthorizationCodeStore<TAuthorizationCode> authorizationCodeStoreService)
    {
        _options = options.Value;
        _cryptographyHelper = cryptographyHelper;
        _authorizationCodeCreationService = authorizationCodeCreationService;
        _authorizationCodeStoreService = authorizationCodeStoreService;
    }

    public async Task<string> CreateCodeAsync(Client client, string[] scopes, string shubject, ClaimsPrincipal claimsPrincipal, RequestData requestData, CancellationToken cancellationToken = default)
    {
        var code = await _authorizationCodeCreationService.CreateAsync(client, scopes, shubject, claimsPrincipal);

        await _authorizationCodeStoreService.CreateAsync(code, client.ClientId, claimsPrincipal, DateTime.UtcNow.Add(_options.DefaultAuthorizationCodeLifetime), requestData);

        return code;
    }

    public async Task<string> GetSubjectAsync(Client client, string[] scopes, string code, CancellationToken cancellationToken = default)
    {
        var authorizationCode = await _authorizationCodeStoreService.FindAsync(code, cancellationToken);
        if (authorizationCode == null)
            return String.Empty;

        return await _authorizationCodeStoreService.GetSubjectAsync(authorizationCode, cancellationToken);
    }

    public async Task<AuthorizationCodeValidationResult> ValidationAsync(Client client, string code, RequestData requestData, CancellationToken cancellationToken = default)
    {
        var authorizationCode = await _authorizationCodeStoreService.FindAsync(code, cancellationToken);

        if (authorizationCode == null)
            return AuthorizationCodeValidationResult.Fail(new Exception("Invalid code."));

        var expiration = await _authorizationCodeStoreService.GetExpirationAsync(authorizationCode, cancellationToken);
        if (expiration < DateTime.UtcNow)
            return AuthorizationCodeValidationResult.Fail(new Exception("Authorization code has expired."));

        var codeChallenge = await _authorizationCodeStoreService.GetCodeChallengeAsync(authorizationCode);
        var codeChallengeMethod = await _authorizationCodeStoreService.GetCodeChallengeMethodAsync(authorizationCode);
        if (!string.IsNullOrEmpty(codeChallenge) && !string.IsNullOrEmpty(codeChallengeMethod))
        {
            if (codeChallengeMethod == "S256")
            {
                var newValue = Base64Helper.ToBase64String(_cryptographyHelper.Sha256(Encoding.ASCII.GetBytes(requestData.CodeVerifier)));

                if (!Base64Helper.Compare(newValue, codeChallenge))
                    return AuthorizationCodeValidationResult.Fail(new Exception("Invalid code."));
            }
            else if (codeChallenge != requestData.CodeVerifier)
            {
                return AuthorizationCodeValidationResult.Fail(new Exception("Invalid code."));
            }
        }

        return AuthorizationCodeValidationResult.Success();
    }
}
