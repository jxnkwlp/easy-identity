using System.Security.Claims;

namespace EasyIdentity.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetSubject(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
