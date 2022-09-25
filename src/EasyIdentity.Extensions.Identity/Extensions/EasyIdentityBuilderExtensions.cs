using EasyIdentity.Extensions.Identity.Services;

namespace EasyIdentity.Extensions.Identity.Extensions;

public static class EasyIdentityBuilderExtensions
{
    /// <summary>
    ///  Integrated Microsoft.Extensions.Identity
    /// </summary> 
    public static EasyIdentityBuilder UseIdentity<TUser>(this EasyIdentityBuilder builder) where TUser : class
    {
        builder.AddUserService<UserService<TUser>>();

        return builder;
    }

    public static EasyIdentityBuilder UseIdentityUserService<TUser>(this EasyIdentityBuilder builder) where TUser : class
    {
        builder.AddUserService<UserService<TUser>>();
        return builder;
    }

}
