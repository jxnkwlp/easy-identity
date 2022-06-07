using EasyIdentity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EasyIdentityBasic.Pages.Account
{
    [Authorize]
    public class DeviceModel : PageModel
    {
        private readonly IAuthorizationInteractionService _authorizationInteractionService;

        public DeviceModel(IAuthorizationInteractionService authorizationInteractionService)
        {
            _authorizationInteractionService = authorizationInteractionService;
        }

        public void OnGet()
        {
        }

        public async Task OnPostAsync(string code)
        {
            var result = await _authorizationInteractionService.DeviceCodeAuthorizationAsync(code);

            if (result.Successed)
            {
                Redirect("~/");
            }
            else
            {
                TempData["error"] = "Invalid Code";
            }
        }
    }
}
