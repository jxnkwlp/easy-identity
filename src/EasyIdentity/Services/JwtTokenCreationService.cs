using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EasyIdentity.Services;

/// <summary>
///  JWT Token generation service 
/// </summary>
public class JwtTokenCreationService : ITokenCreationService
{
    private readonly EasyIdentityOptions _options;
    protected readonly ISigningCredentialsStore _signingCredentialsStore;

    public JwtTokenCreationService(IOptions<EasyIdentityOptions> options, ISigningCredentialsStore signingCredentialsStore)
    {
        _options = options.Value;
        _signingCredentialsStore = signingCredentialsStore;
    }

    public async Task<string> CreateAccessTokenAsync(TokenDescriptor tokenDescriptor)
    {
        var signingCredentials = await _signingCredentialsStore.GetSigningCredentialsAsync(tokenDescriptor.Client);

        if (signingCredentials?.Any() == false)
        {
            throw new Exception("The signing credentials is empty.");
        }

        var handler = new JwtSecurityTokenHandler();

        var additional = new Dictionary<string, object>
        {
        };

        var claims = tokenDescriptor.Principal.Claims.ToList();
        if (tokenDescriptor.Principal.HasClaim(x => x.Type == ClaimTypes.NameIdentifier))
        {
            claims.Add(new Claim(StandardClaimTypes.Sub, tokenDescriptor.Principal.FindFirstValue(ClaimTypes.NameIdentifier)));
        }

        var token = handler.CreateToken(new SecurityTokenDescriptor
        {
            AdditionalHeaderClaims = additional,
            SigningCredentials = signingCredentials[0],
            // Claims = tokenDescriptor.Principal.Claims,
            Subject = new System.Security.Claims.ClaimsIdentity(claims),
            Audience = tokenDescriptor.Audiences,
            IssuedAt = tokenDescriptor.CreationTime,
            Issuer = tokenDescriptor.Issuer,
            TokenType = tokenDescriptor.TokenType,
            Expires = tokenDescriptor.CreationTime.Add(tokenDescriptor.Lifetime),
            NotBefore = tokenDescriptor.CreationTime,
        });

        return handler.WriteToken(token);
    }

    public Task<string> CreateIdentityTokenAsync(TokenDescriptor tokenDescriptor, string accessToken)
    {
        // TODO
        throw new NotImplementedException();
    }

    public Task<string> CreateRefreshTokenAsync(TokenDescriptor tokenDescriptor, string accessToken)
    {
        // TODO
        return Task.FromResult(Guid.NewGuid().ToString("N"));
    }
}
