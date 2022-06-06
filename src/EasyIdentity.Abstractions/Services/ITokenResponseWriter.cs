using System.Threading.Tasks;
using EasyIdentity.Models;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Services
{
    public interface ITokenResponseWriter
    {
        Task WriteAsync(HttpContext context, GrantTypeHandleResult result);
    }
}
