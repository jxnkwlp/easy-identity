using System;

namespace EasyIdentity.Models;

public class AuthorizationCodeValidationResult
{
    public bool Succeeded => Failure == null;

    public Exception Failure { get; protected set; }


    public static AuthorizationCodeValidationResult Fail(Exception exception)
    {
        return new AuthorizationCodeValidationResult
        {
            Failure = exception
        };
    }

    public static AuthorizationCodeValidationResult Success()
    {
        return new AuthorizationCodeValidationResult { };
    }
}
