using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Endpoints
{
    public interface IEndpointHandler
    {
        string Path { get; }
        string[] Methods { get; }

        Task HandleAsync(HttpContext context);
    }
}
