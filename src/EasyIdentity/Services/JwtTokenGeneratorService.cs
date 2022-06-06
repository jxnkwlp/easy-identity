using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.IdentityModel.Tokens;

namespace EasyIdentity.Services
{
    public class JwtTokenGeneratorService : ITokenCreationService
    {
        private readonly EasyIdentityOptionsBuilder _identityOptionsBuilder;

        public JwtTokenGeneratorService(EasyIdentityOptionsBuilder identityOptionsBuilder)
        {
            _identityOptionsBuilder = identityOptionsBuilder;
        }

        public Task<string> CreateRefreshTokenAsync(TokenDescriptor tokenDescriptor)
        {
            return Task.FromResult(Guid.NewGuid().ToString("N"));
        }

        public Task<string> CreateTokenAsync(TokenDescriptor tokenDescriptor)
        {
            var handler = new JwtSecurityTokenHandler();

            //claims[JwtRegisteredClaimNames.Typ] = "Bearer";
            //claims[JwtRegisteredClaimNames.Sub] = tokenDescriptor.Subject;
            //claims[JwtRegisteredClaimNames.AuthTime] = tokenDescriptor.IssuedAt.ToUnixTimeSeconds();
            //claims[JwtRegisteredClaimNames.Iat] = tokenDescriptor.IssuedAt.ToUnixTimeSeconds();
            //claims[JwtRegisteredClaimNames.Exp] = tokenDescriptor.Expires?.ToUnixTimeSeconds();

            var additional = new Dictionary<string, object>
            {
                ["typ"] = tokenDescriptor.TokenType,
            };

            // var payload = JsonSerializer.Serialize(claims);

            if (_identityOptionsBuilder.Options.SigningCredentials.Count > 0)
            {
                var signingKey = _identityOptionsBuilder.Options.SigningCredentials[0];

                var token = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Subject = tokenDescriptor.Identity,
                    AdditionalHeaderClaims = additional,
                    Audience = tokenDescriptor.Client.ClientId,
                    SigningCredentials = signingKey,
                    Expires = tokenDescriptor.CreationTime.AddSeconds(tokenDescriptor.Lifetime),
                    IssuedAt = tokenDescriptor.CreationTime,
                    Issuer = "http://fdafdsaf",
                    //Subject = new System.Security.Claims.ClaimsIdentity(claimsValue.Select(x => new System.Security.Claims.Claim(x.Key, x.Value?.ToString()))),
                    TokenType = tokenDescriptor.TokenType,
                });

                return Task.FromResult(handler.WriteToken(token));
            }

            throw new Exception();
        }
    }
}
