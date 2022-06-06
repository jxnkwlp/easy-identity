using System.Security.Claims;
using EasyIdentity.Models;
using EasyIdentity.Services;

namespace EasyIdentityBasic
{
    public class TestUserProfileService : IUserProfileService
    {
        public Task<UserProfileResult> GetAsync(UserProfileRequest request)
        {
            var claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "bob"),
                new Claim(ClaimTypes.Email, "bob@example.com"),
                new Claim(ClaimTypes.GivenName, "bob"),
            };

            return Task.FromResult(UserProfileResult.Success("bob", new System.Security.Claims.ClaimsIdentity(claims)));
        }
    }
}
