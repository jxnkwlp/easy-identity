using System.Collections.Generic;

namespace EasyIdentity.Models;

/// <summary>
///  The common identity error model
/// </summary>
public class IdentityError : Dictionary<string, string>
{
    public const string Client_Not_Found = "client_invaid";

    public string Error { get; }

    public string Description { get; }

    protected IdentityError()
    {
    }

    public IdentityError(string error, string description)
    {
        Error = error;
        Description = description;
    }

    public static IdentityError Create(string error, string description = null)
    {
        return new IdentityError(error, description);
    }
}
