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
        public string Path => EndpointProtocolRoutePaths.Token;

        public string[] Methods => new string[1] { HttpMethods.Post };

        private readonly IRequestParamReader _requestParamReader;
        private readonly IEnumerable<IGrantTypeHandler> _grantTypeHandlers;
        private readonly ITokenRequestValidator _tokenRequestValidator;
        private readonly IResponseWriter _responseWriter;

        public TokenEndpointHandler(IRequestParamReader requestParamReader, IEnumerable<IGrantTypeHandler> grantTypeHandlers, ITokenRequestValidator tokenRequestValidator, IResponseWriter responseWriter)
        {
            _requestParamReader = requestParamReader;
            _grantTypeHandlers = grantTypeHandlers;
            _tokenRequestValidator = tokenRequestValidator;
            _responseWriter = responseWriter;
        }

        public async Task HandleAsync(HttpContext context)
        {
            var requestData = await _requestParamReader.ReadAsync();

            var grantType = requestData.GrantType;

            var validationResult = await _tokenRequestValidator.ValidateAsync(grantType, requestData);

            if (!validationResult.Succeeded)
            {
                await _responseWriter.WriteAsync(new ResponseDescriptor(requestData, validationResult.Error, validationResult.ErrorDescription));
                return;
            }

            var grantTypeHandleRequest = new GrantTypeHandleRequest(null, validationResult.Client, null, requestData);

            var grantTypeHandler = _grantTypeHandlers.FirstOrDefault(x => x.GrantType == validationResult.GrantType);

            if (grantTypeHandler == null)
                throw new Exception($"The grant type '{validationResult.GrantType}' is not supported");

            var result = await grantTypeHandler.HandleAsync(grantTypeHandleRequest);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(result.HttpLocation))
                    await _responseWriter.WriteAsync(new ResponseDescriptor(requestData, result.HttpLocation));
                else
                    await _responseWriter.WriteAsync(new ResponseDescriptor(requestData, result.Token.ToDictionary()));
            }
            else
            {
                await _responseWriter.WriteAsync(new ResponseDescriptor(requestData, result.Failure));
            }
        }
    }
}
