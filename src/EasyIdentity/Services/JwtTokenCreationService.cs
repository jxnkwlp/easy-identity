using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.IdentityModel.Tokens;

namespace EasyIdentity.Services
{
    /// <summary>
    ///  JWT Token generation service 
    /// </summary>
    public class JwtTokenCreationService : ITokenCreationService
    {
        private readonly EasyIdentityOptions _options;

        public JwtTokenCreationService(EasyIdentityOptions options)
        {
            _options = options;
        }

        public Task<string> CreateAccessTokenAsync(TokenDescriptor tokenDescriptor)
        {
            var handler = new JwtSecurityTokenHandler();

            var additional = new Dictionary<string, object>
            {
            };

            var claims = tokenDescriptor.Principal.Claims.ToList();
            if (tokenDescriptor.Principal.HasClaim(x => x.Type == ClaimTypes.NameIdentifier))
            {
                claims.Add(new Claim(StandardClaimTypes.Sub, tokenDescriptor.Principal.FindFirstValue(ClaimTypes.NameIdentifier)));
            }

            if (_options.SigningCredentials.Count > 0)
            {
                var signingKey = _options.SigningCredentials[0];

                var token = handler.CreateToken(new SecurityTokenDescriptor
                {
                    AdditionalHeaderClaims = additional,
                    SigningCredentials = signingKey,
                    // Claims = tokenDescriptor.Principal.Claims,
                    Subject = new System.Security.Claims.ClaimsIdentity(claims),
                    Audience = tokenDescriptor.Audiences,
                    IssuedAt = tokenDescriptor.CreationTime,
                    Issuer = tokenDescriptor.Issuer,
                    TokenType = tokenDescriptor.TokenType,
                    Expires = tokenDescriptor.CreationTime.Add(tokenDescriptor.Lifetime),
                    NotBefore = tokenDescriptor.CreationTime,
                });

                return Task.FromResult(handler.WriteToken(token));
            }
            else
            {
                throw new Exception("The signing credentials is empty.");
            }
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
}
