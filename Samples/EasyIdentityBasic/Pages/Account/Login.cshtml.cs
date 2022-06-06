using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EasyIdentityBasic.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        public void OnGet()
        {

        }

        public async void OnPost(string username, string password, string returnUrl)
        {
            if (username != "bob")
            {
                TempData["error"] = "Invalid username";
            }
            else
            {
                var claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()));
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, "bob"));

                var authProperties = new AuthenticationProperties();

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    Redirect(returnUrl);
                }
                else
                {
                    Redirect("/");
                }
            }
        }
    }
}
