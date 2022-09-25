using System;

namespace EasyIdentity.Models;

public class GrantTypeExecutionResult
{
    public bool Succeeded => Failure == null;

    public Exception Failure { get; protected set; }

    public TokenData Token { get; protected set; }

    public string HttpLocation { get; protected set; }

    public static GrantTypeExecutionResult Success(TokenData token)
    {
        return new GrantTypeExecutionResult() { Token = token };
    }

    public static GrantTypeExecutionResult Success(TokenCreationResult token)
    {
        return new GrantTypeExecutionResult()
        {
            Token = new TokenData
            {
                Scope = string.Join(" ", token.AccessTokenDescriptor.Client.Scopes),
                TokenType = token.AccessTokenDescriptor.TokenType,

                AccessToken = token.AccessToken,
                ExpiresIn = (int)token.AccessTokenDescriptor.Lifetime.TotalSeconds,

                RefreshToken = token.RefreshToken,
                RefreshTokenExpiresIn = (int?)token.RefreshTokenDescriptor?.Lifetime.TotalSeconds ?? null,

                IdToken = token.IdentityToken,
                IdTokenExpiresIn = (int?)token.IdentityTokenDescriptor?.Lifetime.TotalSeconds ?? null,
            }
        };
    }

    public static GrantTypeExecutionResult Success(string httpLocation)
    {
        if (string.IsNullOrWhiteSpace(httpLocation))
        {
            throw new ArgumentException($"'{nameof(httpLocation)}' cannot be null or whitespace.", nameof(httpLocation));
        }

        return new GrantTypeExecutionResult() { HttpLocation = httpLocation };
    }

    public static GrantTypeExecutionResult Fail(Exception failure)
    {
        if (failure == null)
            throw new ArgumentNullException(nameof(failure));

        return new GrantTypeExecutionResult { Failure = failure };
    }
}
