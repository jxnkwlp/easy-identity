using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyIdentity.Models;
using EasyIdentity.Services;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Endpoints
{
    public class TokenEndpointHandler : IEndpointHandler
    {
        public string Path => ProtocolRoutePaths.Token;

        public string[] Methods => new string[1] { HttpMethods.Post };

        private readonly IRequestParamReader _parser;
        private readonly IEnumerable<IGrantTypeHandler> _grantTypeHandlers;
        private readonly ITokenRequestValidator _tokenRequestValidator;
        private readonly ITokenResponseWriter _tokenResponseWriter;

        public TokenEndpointHandler(IRequestParamReader parser, IEnumerable<IGrantTypeHandler> grantTypeHandlers, ITokenRequestValidator tokenRequestValidator, ITokenResponseWriter tokenResponseWriter)
        {
            _parser = parser;
            _grantTypeHandlers = grantTypeHandlers;
            _tokenRequestValidator = tokenRequestValidator;
            _tokenResponseWriter = tokenResponseWriter;
        }

        public async Task HandleAsync(HttpContext context)
        {
            var requestData = await _parser.ReadAsync();
            var grantType = requestData["grant_type"];
            var validatedResult = await _tokenRequestValidator.ValidateAsync(grantType, requestData);

            if (!validatedResult.Succeeded)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new { error = validatedResult.Error, error_description = validatedResult.ErrorDescription });
                return;
            }

            var handleContext = new GrantTypeHandleRequest(validatedResult.Client, null, requestData.Data);

            var handle = _grantTypeHandlers.FirstOrDefault(x => x.GrantType == validatedResult.GrantType);

            if (handle == null)
                throw new Exception($"The grant type '{validatedResult.GrantType}' is not supported");

            // handle the token 
            var result = await handle.HandleAsync(handleContext);

            if (result.HasError == false)
            {
                if (!string.IsNullOrEmpty(result.ResponseLocation))
                {
                    context.Response.Redirect(result.ResponseLocation);
                    return;
                }
                else
                {
                    context.Response.StatusCode = 200;
                }
            }
            else
            {
                context.Response.StatusCode = 400;
            }

            await _tokenResponseWriter.WriteAsync(context, result);
        }
    }
}
