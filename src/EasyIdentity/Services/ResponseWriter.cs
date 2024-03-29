﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Services;

public class ResponseWriter : IResponseWriter
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ResponseWriter(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task WriteAsync(ResponseDescriptor descriptor)
    {
        var context = _httpContextAccessor.HttpContext;
        var request = context.Request;
        var response = context.Response;

#if NET6_0_OR_GREATER
        context.Response.Headers.CacheControl = "no-store";
#elif NET5_0
        context.Response.Headers["Cache-Control"] = "no-store";
#endif

        // TODO: response.HasStarted

        var responseMode = descriptor.RequestData.ResponseMode;

        if (!descriptor.Succeeded)
        {
            response.StatusCode = 400;

            await response.WriteAsJsonAsync(new
            {
                error = descriptor.Error,
                error_description = descriptor.ErrorDescription,
                timestamp = DateTime.UtcNow.ToString("u"),
                trace_id = Activity.Current?.Id ?? context.TraceIdentifier
            });

            return;
        }

        if (!string.IsNullOrEmpty(descriptor.HttpLocation))
        {
            response.Redirect(descriptor.HttpLocation, false);

            return;
        }

        if (descriptor.Data != null)
        {
            await response.WriteAsJsonAsync(descriptor.Data);
        }
        else
        {
            // no-op
        }
    }
}
