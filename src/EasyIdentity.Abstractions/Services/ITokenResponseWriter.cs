using System;
using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Services;

[Obsolete]
public interface ITokenResponseWriter
{
    Task WriteAsync(HttpContext context, GrantTypeExecutionResult result);
}
