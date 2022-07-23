using System;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services
{
    public class ImplicitGrantTypeHandler : IGrantTypeHandler
    {
        public string GrantType => GrantTypesConsts.Implicit;

        private readonly IUserService _userService;
        private readonly ITokenManager _tokenManager;

        public ImplicitGrantTypeHandler(IUserService userService, ITokenManager tokenManager)
        {
            _userService = userService;
            _tokenManager = tokenManager;
        }

        public async Task<GrantTypeHandledResult> HandleAsync(GrantTypeHandleRequest request)
        {
            var client = request.Client;
            var subject = request.Subject;

            var userProfile = await _userService.GetProfileAsync(new UserProfileRequest(client, subject, request.Data));

            if (userProfile.Locked)
            {
                return GrantTypeHandledResult.Fail(new Exception("access_denied"));
            }

            var token = await _tokenManager.CreateAsync(subject, client, userProfile.Principal);

            string url = $"{request.Data.RedirectUri}?#access_token={token.AccessToken}&token_type=Bearer&scope={string.Join(" ", client.Scopes)}&state={request.Data.State}&expires_in={(int)token.TokenDescriptor.Lifetime.TotalSeconds}";

            return GrantTypeHandledResult.Success(url);
        }
    }
}
