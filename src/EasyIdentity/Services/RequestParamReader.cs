using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Services;

public class RequestParamReader : IRequestParamReader
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RequestParamReader(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private static bool TryDecodeBase64String(string value, out string source)
    {
        source = string.Empty;
        Span<byte> buffer = new Span<byte>(new byte[value.Length]);
        if (Convert.TryFromBase64String(value, buffer, out int length))
        {
            source = Encoding.UTF8.GetString(buffer.Slice(0, length));
            return true;
        }

        return false;
    }

    public async Task<RequestData> ReadAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        var request = httpContext.Request;

        var data = new Dictionary<string, string>();

        foreach (var item in request.Query)
        {
            data[item.Key] = item.Value;
        }

        if (request.Method == HttpMethods.Post && request.HasFormContentType)
        {
            var formData = await request.ReadFormAsync();

            foreach (var item in formData)
            {
                data[item.Key] = item.Value;
            }
        }

        var authorization = request.Headers["Authorization"].ToString();

        // request.Headers.Authorization.ToArray();

        if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("basic", StringComparison.InvariantCultureIgnoreCase))
        {
            if (TryDecodeBase64String(authorization.Substring(5).Trim(), out var value))
            {
                if (value.IndexOf(":") > 0)
                {
                    data["client_id"] = value.Substring(0, value.IndexOf(":"))?.Trim();
                    data["client_secret"] = value.Substring(value.IndexOf(":") + 1)?.Trim();
                }
            }
        }

        return new RequestData(data);
    }
}
