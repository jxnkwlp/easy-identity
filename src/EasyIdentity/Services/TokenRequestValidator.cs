using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyIdentity.Models;

namespace EasyIdentity.Services;

public class TokenRequestValidator : ITokenRequestValidator
{
    private readonly IEnumerable<IGrantTypeTokenRequestValidator> _validators;

    public TokenRequestValidator(IEnumerable<IGrantTypeTokenRequestValidator> validators)
    {
        _validators = validators;
    }

    public async Task<RequestValidationResult> ValidateAsync(string grantType, RequestData requestData)
    {
        var validator = _validators.FirstOrDefault(x => x.GrantType == grantType);

        if (validator == null)
            throw new Exception($"The grant type '{grantType}' validator not found.");

        return await validator.ValidateAsync(requestData);
    }
}
