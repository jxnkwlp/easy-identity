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
                claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "1"));
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, "bob"));

                var authProperties = new AuthenticationProperties();

                await HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity), authProperties);

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
