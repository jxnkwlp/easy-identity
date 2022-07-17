using System.Security.Claims;
using EasyIdentity.Models;
using EasyIdentity.Services;

namespace EasyIdentityServerBasic
{
    public class TestUserService : IUserService
    {
        public Task<UserProfileResult> GetProfileAsync(UserProfileRequest request)
        {
            var claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "bob"),
                new Claim(ClaimTypes.Email, "bob@example.com"),
                new Claim(ClaimTypes.GivenName, "bob"),
            };

            var identity = new ClaimsIdentity(claims);

            return Task.FromResult(UserProfileResult.Success("1", new ClaimsPrincipal(identity)));
        }

        public Task<string> GetSubjectAsync(string username, string password, RequestData requestData)
        {
            if (username == "bob")
                return Task.FromResult("1");
            return Task.FromResult<string>(string.Empty);
        }
    }
}
