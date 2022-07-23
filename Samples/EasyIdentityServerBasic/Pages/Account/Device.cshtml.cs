using EasyIdentity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EasyIdentityBasic.Pages.Account;

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

    public async Task OnPostAsync(string code, bool grant = false)
    {
        var result = await _authorizationInteractionService.DeviceUserCodeAuthorizationAsync(code, User, grant);

        if (result)
        {
            TempData["success"] = grant ? "Authorized!" : "Denied!";
            Redirect("~/");
        }
        else
        {
            TempData["error"] = "Invalid Code";
        }
    }
}
