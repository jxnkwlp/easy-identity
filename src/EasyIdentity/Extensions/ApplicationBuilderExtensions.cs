using EasyIdentity.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace EasyIdentity.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseEasyIdentity(this IApplicationBuilder app)
        {
            var routeBuilder = new RouteBuilder(app);

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var handlers = scope.ServiceProvider.GetServices<IEndpointHandler>();

                foreach (var handler in handlers)
                {
                    foreach (var method in handler.Methods)
                    {
                        routeBuilder.MapVerb(method, handler.Path, async (context) =>
                        {
                            await handler.HandleAsync(context);
                        });
                    }
                }
            }

            app.UseRouter(routeBuilder.Build());

            return app;
        }
    }
}
