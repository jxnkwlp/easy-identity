using System;
using System.Threading;
using System.Threading.Tasks;
using EasyIdentity.Models;
using EasyIdentity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EasyIdentity.Extensions.Identity.Services;

public class UserService<TUser> : IUserService where TUser : class
{
    private readonly ILogger<UserService<TUser>> _logger;
    private readonly UserManager<TUser> _userManager;
    private readonly SignInManager<TUser> _signInManager;

    public UserService(ILogger<UserService<TUser>> logger, UserManager<TUser> userManager, SignInManager<TUser> signInManager)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<UserProfileResult> GetProfileAsync(UserProfileRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(request.Subject);

        if (user == null)
            return null;

        if (await _userManager.IsLockedOutAsync(user))
        {
            return UserProfileResult.UserLocked();
        }

        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);

        return UserProfileResult.Success(request.Subject, claimsPrincipal);
    }

    public async Task<string> GetSubjectAsync(string username, string password, RequestData requestData, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user == null)
        {
            return String.Empty;
        }

        if (await _userManager.CheckPasswordAsync(user, password))
        {
            return await _userManager.GetUserIdAsync(user);
        }
        else
        {
            return String.Empty;
        }
    }
}
