﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EasyIdentity.Extensions;
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
    private readonly IAuthorizationCodeFlowManager _authorizationCodeManager;

    public AuthorizationEndpointHandler(IOptions<EasyIdentityOptions> easyIdentityOptions, IRequestParamReader requestParamReader, IEnumerable<IGrantTypeHandler> grantTypeHandlers, ITokenRequestValidator tokenRequestValidator, IResponseWriter responseWriter, IAuthorizationRequestValidator authorizationRequestValidator, IAuthorizationCodeFlowManager authorizationCodeManager)
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
            var responseTypes = requestData.ResponseType.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            if (responseTypes.Length == 1 && responseTypes[0] == "code")
            {
                await AuthorizationCodeFlowAsync(context, requestData, validationResult.Client, result);
            }
            else if (responseTypes.Contains("token") || responseTypes.Contains("id_token"))
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
        if (!client.GrantTypes.Contains(GrantTypeNameConsts.AuthorizationCode))
        {
            await _responseWriter.WriteAsync(new ResponseDescriptor(requestData, "invaid_grant_type"));
        }
        else
        {
            var subject = result.Principal.GetSubject();

            var code = await _authorizationCodeManager.CreateCodeAsync(client, requestData.Scopes, subject, result.Principal, requestData);

            await _responseWriter.WriteAsync(new ResponseDescriptor(requestData, $"{requestData.RedirectUri}?code={code}&state={requestData.State}"));
        }
    }

    private async Task ImplicitFlowAsync(HttpContext httpContext, RequestData requestData, AuthenticateResult authenticateResult)
    {
        var validationResult = await _tokenRequestValidator.ValidateAsync(GrantTypeNameConsts.Implicit, requestData);

        if (!validationResult.Succeeded)
        {
            await _responseWriter.WriteAsync(new ResponseDescriptor(requestData, validationResult.Error, validationResult.ErrorDescription));
            return;
        }

        var grantTypeHandler = _grantTypeHandlers.FirstOrDefault(x => x.GrantType == GrantTypeNameConsts.Implicit);

        if (grantTypeHandler == null)
            throw new Exception($"The grant type '{validationResult.GrantType}' is not supported");

        var subject = authenticateResult.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(subject))
        {
            throw new Exception("The user identifiter not found.");
        }

        var result = await grantTypeHandler.ExecuteAsync(new GrantTypeExecutionRequest(subject, validationResult.Client, authenticateResult.Principal, requestData));

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
