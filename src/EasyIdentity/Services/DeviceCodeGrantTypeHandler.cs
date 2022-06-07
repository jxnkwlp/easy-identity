using System;
using System.Linq;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services
{
    public class DeviceCodeGrantTypeHandler : IGrantTypeHandler
    {
        public string GrantType => GrantTypesConsts.DeviceCode;

        private readonly IDeviceCodeStoreService _deviceCodeStoreService;
        private readonly IUserProfileService _userProfileService;
        private readonly ITokenCreationService _tokenGeneratorService;

        public async Task<GrantTypeHandleResult> HandleAsync(GrantTypeHandleRequest context)
        {
            var client = context.Client;

            var result = new GrantTypeHandleResult();

            var deviceCode = context.RawData["device_code"];
            var subject = await _deviceCodeStoreService.GetSubjectAsync(deviceCode);

            if (string.IsNullOrWhiteSpace(subject))
            {
                result.SetError("Invalid code");
                return result;
            }

            await _deviceCodeStoreService.RemoveAsync(deviceCode);

            var userProfile = await _userProfileService.GetAsync(new UserProfileRequest(context.RawData, client, subject));

            if (userProfile.Succeeded == false)
            {
                result.SetError(userProfile.Error, userProfile.ErrorDescription);
                return result;
            }

            // TODO 
            var tokenDescriptor = new TokenDescriptor(subject, client)
            {
                TokenType = "JWT",
                CreationTime = DateTime.UtcNow,
                Lifetime = 300,
                Identity = new System.Security.Claims.ClaimsIdentity(userProfile.Identity.Claims, ".easyidentity", "sub", "roles"),
            };

            if (tokenDescriptor.Identity.HasClaim(x => x.Type == "sub"))
            {
                tokenDescriptor.Identity.TryRemoveClaim(tokenDescriptor.Identity.FindFirst(x => x.Type == "sub"));
            }
            tokenDescriptor.Identity.AddClaim(new System.Security.Claims.Claim("sub", userProfile.SubjectId));

            var accessToken = await _tokenGeneratorService.CreateTokenAsync(tokenDescriptor);

            // id token
            if (client.Scopes.Contains(StandardScopes.OpenId))
            {
                // TODO 
            }

            string refreshToken = null;
            if (client.Scopes.Contains(StandardScopes.OfflineAccess))
            {
                refreshToken = await _tokenGeneratorService.CreateRefreshTokenAsync(tokenDescriptor);
            }

            result.ResponseData = new TokenResponseData
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Scope = string.Join(" ", client.Scopes),
                ExpiresIn = tokenDescriptor.Lifetime,
                TokenType = "Bearer",
            };

            return result;
        }
    }
}
