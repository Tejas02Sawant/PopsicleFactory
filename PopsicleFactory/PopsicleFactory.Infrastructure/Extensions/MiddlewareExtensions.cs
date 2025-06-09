using Microsoft.AspNetCore.Builder;
using PopsicleFactory.Infrastructure.Middlewares;
using Serilog;

namespace PopsicleFactory.Infrastructure.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseCustomMiddlewares(this IApplicationBuilder app)
    {
        return app
            .UseMiddleware<RequestContextLoggingMiddleware>()
            .UseSerilogRequestLogging();
            // Add more here as needed
    }
}
