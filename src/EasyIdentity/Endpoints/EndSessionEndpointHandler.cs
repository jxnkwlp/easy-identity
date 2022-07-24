using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Endpoints;

public class EndSessionEndpointHandler : IEndpointHandler
{
    public string Path => EndpointProtocolRoutePaths.EndSession;

    public string[] Methods => new string[] { HttpMethods.Get, HttpMethods.Post };

    public Task HandleAsync(HttpContext context)
    {
        throw new NotImplementedException();
    }
}
