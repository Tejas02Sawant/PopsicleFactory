using PopsicleFactory.WebApi.Services;

namespace PopsicleFactory.WebApi.Helpers;

public static class ConfigureServices
{
    public static IServiceCollection AddInjectionServices(this IServiceCollection services)
    {
        services.AddScoped<IPopsicleService, PopsicleService>();
        
        return services;
    }
}
