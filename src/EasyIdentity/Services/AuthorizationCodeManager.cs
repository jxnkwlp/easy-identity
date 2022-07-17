using System;
using System.Security.Claims;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services
{
    public class AuthorizationCodeManager : IAuthorizationCodeManager
    {
        private readonly IAuthorizationCodeCreationService _authorizationCodeCreationService;
        private readonly IAuthorizationCodeStoreService _authorizationCodeStoreService;
        private readonly EasyIdentityOptions _options;

        public AuthorizationCodeManager(IAuthorizationCodeCreationService authorizationCodeCreationService, IAuthorizationCodeStoreService authorizationCodeStoreService, EasyIdentityOptions options)
        {
            _authorizationCodeCreationService = authorizationCodeCreationService;
            _authorizationCodeStoreService = authorizationCodeStoreService;
            _options = options;
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
}
