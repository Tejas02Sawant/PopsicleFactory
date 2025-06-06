using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PopsicleFactory.DataProvider.Entities.Contexts;
using PopsicleFactory.DataProvider.Entities.Repositories;

namespace PopsicleFactory.DataProvider.Entities.Contexts;

public static class ConfigureServices
{
    public static IServiceCollection AddInjectionDataProvider(this IServiceCollection services)
    {
        services.AddDbContext<RepositoryContext>(opt => opt.UseInMemoryDatabase("InMem"));
        services.AddScoped<IPopsicleRepository, PopsicleRepository>();
        
        return services;
    }
}
