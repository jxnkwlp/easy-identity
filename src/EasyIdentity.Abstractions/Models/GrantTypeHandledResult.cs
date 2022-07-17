using System;

namespace EasyIdentity.Models
{
    public class GrantTypeHandledResult
    {
        public bool Succeeded => Failure == null;

        public Exception Failure { get; protected set; }

        public TokenData Token { get; protected set; }

        public string HttpLocation { get; protected set; }

        public static GrantTypeHandledResult Success(TokenData token)
        {
            return new GrantTypeHandledResult() { Token = token };
        }

        public static GrantTypeHandledResult Success(string httpLocation)
        {
            if (string.IsNullOrWhiteSpace(httpLocation))
            {
                throw new ArgumentException($"'{nameof(httpLocation)}' cannot be null or whitespace.", nameof(httpLocation));
            }

            return new GrantTypeHandledResult() { HttpLocation = httpLocation };
        }

        public static GrantTypeHandledResult Fail(Exception failure)
        {
            if (failure == null)
                throw new ArgumentNullException(nameof(failure));

            return new GrantTypeHandledResult { Failure = failure };
        }
    }
}
