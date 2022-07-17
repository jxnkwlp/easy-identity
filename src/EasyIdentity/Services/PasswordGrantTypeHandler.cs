using System;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services
{
    public class PasswordGrantTypeHandler : IGrantTypeHandler
    {
        public string GrantType => GrantTypesConsts.Password;

        private readonly IUserService _userService;
        private readonly ITokenManager _tokenManager;

        public PasswordGrantTypeHandler(IUserService userService, ITokenManager tokenManager)
        {
            _userService = userService;
            _tokenManager = tokenManager;
        }

        public async Task<GrantTypeHandledResult> HandleAsync(GrantTypeHandleRequest request)
        {
            var client = request.Client;
            var requestData = request.Data;

            var subject = await _userService.GetSubjectAsync(requestData.Username, requestData.Password, requestData);

            if (string.IsNullOrWhiteSpace(subject))
            {
                return GrantTypeHandledResult.Fail(new Exception("invalid_username"));
            }

            var userProfile = await _userService.GetProfileAsync(new UserProfileRequest(client, subject, request.Data));

            if (userProfile.Locked)
            {
                return GrantTypeHandledResult.Fail(new Exception("access_denied"));
            }

            var token = await _tokenManager.CreateAsync(subject, client, userProfile.Principal);

            return GrantTypeHandledResult.Success(new TokenData
            {
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken,
                Scope = string.Join(" ", client.Scopes),
                ExpiresIn = (int)token.TokenDescriptor.Lifetime.TotalSeconds,
                TokenType = token.TokenDescriptor.TokenType,
            });
        }

    }
}
