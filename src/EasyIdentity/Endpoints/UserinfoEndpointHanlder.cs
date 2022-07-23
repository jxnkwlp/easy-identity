using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EasyIdentity.Endpoints;

public class UserinfoEndpointHanlder : IEndpointHandler
{
    public string Path => EndpointProtocolRoutePaths.UserInfo;

    public string[] Methods => throw new NotImplementedException();

    public Task HandleAsync(HttpContext context)
    {
        throw new NotImplementedException();
    }
}
