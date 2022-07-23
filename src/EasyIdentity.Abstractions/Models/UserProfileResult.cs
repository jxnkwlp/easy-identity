using System.Security.Claims;

namespace EasyIdentity.Models;

public class UserProfileResult
{
    public string Subject { get; protected set; }

    public ClaimsPrincipal Principal { get; protected set; }

    public bool Locked { get; protected set; }

    protected UserProfileResult()
    {
    }

    public static UserProfileResult UserLocked()
    {
        return new UserProfileResult { Locked = true };
    }

    public static UserProfileResult Success(string subject, ClaimsPrincipal principal)
    {
        return new UserProfileResult { Principal = principal, Subject = subject, };
    }
}
