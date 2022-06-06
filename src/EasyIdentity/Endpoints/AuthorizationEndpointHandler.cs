using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyIdentity.Models;
using EasyIdentity.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Endpoints
{
    public class AuthorizationEndpointHandler : IEndpointHandler
    {
        public string Path => ProtocolRoutePaths.Authorize;

        public string[] Methods => new string[2] { HttpMethods.Get, HttpMethods.Post };

        private readonly IRequestParamReader _requestParamReader;
        private readonly IAuthorizationRequestValidator _authorizationRequestValidator;
        private readonly IAuthorizationCodeCreationService _authorizationCodeCreationService;
        private readonly IAuthorizationCodeStoreService _authorizationCodeStoreService;
        private readonly IEnumerable<IGrantTypeHandler> _grantTypeHandlers;
        private readonly ITokenRequestValidator _tokenRequestValidator;
        private readonly ITokenResponseWriter _tokenResponseWriter;

        public AuthorizationEndpointHandler(IRequestParamReader requestParamReader, IAuthorizationRequestValidator authorizationRequestValidator, IAuthorizationCodeCreationService authorizationCodeCreationService, IAuthorizationCodeStoreService authorizationCodeStoreService, IEnumerable<IGrantTypeHandler> grantTypeHandlers, ITokenRequestValidator tokenRequestValidator, ITokenResponseWriter tokenResponseWriter)
        {
            _requestParamReader = requestParamReader;
            _authorizationRequestValidator = authorizationRequestValidator;
            _authorizationCodeCreationService = authorizationCodeCreationService;
            _authorizationCodeStoreService = authorizationCodeStoreService;
            _grantTypeHandlers = grantTypeHandlers;
            _tokenRequestValidator = tokenRequestValidator;
            _tokenResponseWriter = tokenResponseWriter;
        }

        public async Task HandleAsync(HttpContext context)
        {
            var requestData = await _requestParamReader.ReadAsync();

            var responseType = requestData["response_type"];

            var validatedResult = await _authorizationRequestValidator.ValidateAsync(requestData);

            if (!validatedResult.Succeeded)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new { error = validatedResult.Error, error_description = validatedResult.ErrorDescription });
                return;
            }

            var result = await context.AuthenticateAsync();

            if (result.Succeeded)
            {
                if (responseType == "code")
                {
                    await AuthorizationCodeFlowAsync(context, requestData, result);
                }
                else if (responseType == "token")
                {
                    await ImplicitFlowAsync(context, requestData, result);
                }
            }
            else if (result.Failure != null)
            {
                throw result.Failure;
            }
            else
            {
                await context.ChallengeAsync();
            }

        }

        private async Task AuthorizationCodeFlowAsync(HttpContext httpContext, RequestData requestData, AuthenticateResult result)
        {
            var code = await _authorizationCodeCreationService.CreateAsync(result.Principal);

            await _authorizationCodeStoreService.SaveAsync(result.Principal, code, DateTime.UtcNow.AddSeconds(60));

            httpContext.Response.Redirect($"{requestData["redirect_uri"]}?code={code}&state={requestData["state"]}");
        }

        private async Task ImplicitFlowAsync(HttpContext httpContext, RequestData requestData, AuthenticateResult result)
        { 
            var validationResult = await _tokenRequestValidator.ValidateAsync(GrantTypesConsts.Implicit, requestData);

            // TOTO : validationResult

            var handler = _grantTypeHandlers.FirstOrDefault(x => x.GrantType == GrantTypesConsts.Implicit);

            var handleResult = await handler.HandleAsync(new GrantTypeHandleRequest(validationResult.Client, null, requestData.Data));

            if (handleResult.HasError)
            {
                // TODO
            }

            await _tokenResponseWriter.WriteAsync(httpContext, handleResult);
        }

    }
}
