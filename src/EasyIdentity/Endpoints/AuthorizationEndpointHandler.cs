using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EasyIdentity.Models;
using EasyIdentity.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace EasyIdentity.Endpoints;

public class AuthorizationEndpointHandler : IEndpointHandler
{
    public string Path => EndpointProtocolRoutePaths.Authorize;

    public string[] Methods => new string[] { HttpMethods.Get, HttpMethods.Post };


    private readonly EasyIdentityOptions _easyIdentityOptions;

    private readonly IRequestParamReader _requestParamReader;
    private readonly IEnumerable<IGrantTypeHandler> _grantTypeHandlers;
    private readonly ITokenRequestValidator _tokenRequestValidator;
    private readonly IResponseWriter _responseWriter;

    private readonly IAuthorizationRequestValidator _authorizationRequestValidator;
    private readonly IAuthorizationCodeManager _authorizationCodeManager;

    public AuthorizationEndpointHandler(IOptions<EasyIdentityOptions> easyIdentityOptions, IRequestParamReader requestParamReader, IEnumerable<IGrantTypeHandler> grantTypeHandlers, ITokenRequestValidator tokenRequestValidator, IResponseWriter responseWriter, IAuthorizationRequestValidator authorizationRequestValidator, IAuthorizationCodeManager authorizationCodeManager)
    {
        _easyIdentityOptions = easyIdentityOptions.Value;
        _requestParamReader = requestParamReader;
        _grantTypeHandlers = grantTypeHandlers;
        _tokenRequestValidator = tokenRequestValidator;
        _responseWriter = responseWriter;
        _authorizationRequestValidator = authorizationRequestValidator;
        _authorizationCodeManager = authorizationCodeManager;
    }

    public async Task HandleAsync(HttpContext context)
    {
        var requestData = await _requestParamReader.ReadAsync();

        var validationResult = await _authorizationRequestValidator.ValidateAsync(requestData);

        if (!validationResult.Succeeded)
        {
            await _responseWriter.WriteAsync(new ResponseDescriptor(requestData, validationResult.Error, validationResult.ErrorDescription));
            return;
        }

        var result = await context.AuthenticateAsync(_easyIdentityOptions.AuthenticationScheme);

        if (result.Succeeded)
        {
            var responseType = requestData.ResponseType;

            if (responseType == "code")
            {
                await AuthorizationCodeFlowAsync(context, requestData, validationResult.Client, result);
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

    private async Task AuthorizationCodeFlowAsync(HttpContext httpContext, RequestData requestData, Client client, AuthenticateResult result)
    {
        var code = await _authorizationCodeManager.CreateCodeAsync(client, result.Principal);

        await _responseWriter.WriteAsync(new ResponseDescriptor(requestData, $"{requestData.RedirectUri}?code={code}&state={requestData.State}"));
    }

    private async Task ImplicitFlowAsync(HttpContext httpContext, RequestData requestData, AuthenticateResult authenticateResult)
    {
        var validationResult = await _tokenRequestValidator.ValidateAsync(GrantTypesConsts.Implicit, requestData);

        if (!validationResult.Succeeded)
        {
            await _responseWriter.WriteAsync(new ResponseDescriptor(requestData, validationResult.Error, validationResult.ErrorDescription));
            return;
        }

        var grantTypeHandler = _grantTypeHandlers.FirstOrDefault(x => x.GrantType == GrantTypesConsts.Implicit);

        if (grantTypeHandler == null)
            throw new Exception($"The grant type '{validationResult.GrantType}' is not supported");

        var subject = authenticateResult.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(subject))
        {
            throw new Exception("The user identifiter not found.");
        }

        var result = await grantTypeHandler.HandleAsync(new GrantTypeHandleRequest(subject, validationResult.Client, null, requestData));

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
