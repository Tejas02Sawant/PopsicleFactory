using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using PopsicleFactory.Infrastructure.ExceptionHandlers;

namespace PopsicleFactory.Infrastructure.Helpers;

public static class ConfigureServices
{
    public static IServiceCollection AddInjectionInfrastructure(this IServiceCollection services)
    {

        services.AddHealthChecks()
        .AddCheck("default", () => HealthCheckResult.Healthy());

        //Exception handler chain
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddExceptionHandler<BadRequestExceptionHandler>();
        services.AddExceptionHandler<NotFoundExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}
