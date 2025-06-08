using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PopsicleFactory.DataProvider.Entities.Contexts;
using PopsicleFactory.DataProvider.Models;

namespace PopsicleFactory.DataProvider.Entities;

public static class PopsicleDb
{
    public static void PrepPopulation(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateAsyncScope();
        SeedData(serviceScope.ServiceProvider.GetService<RepositoryContext>()!);
    }

    private static void SeedData(RepositoryContext context)
    {
        if(!context.Popsicles.Any())
        {
            Console.WriteLine("--> Seeding data...");

            context.Popsicles.AddRange(
                new Popsicle { Id = Guid.NewGuid(), Flavor = "Strawberry", Color = "Red", Quantity = 50, Price = 2 },
                new Popsicle { Id = Guid.NewGuid(), Flavor = "Blue Raspberry", Color = "Blue", Quantity = 30, Price = 2.49m },
                new Popsicle { Id = Guid.NewGuid(), Flavor = "Lime", Color = "Green", Quantity = 20, Price = 1.49m },
                new Popsicle { Id = Guid.NewGuid(), Flavor = "Mango", Color = "Orange", Quantity = 40, Price = 3 }
            );

            context.SaveChanges();
        } else{
            Console.WriteLine("--> We already have data");
        }
    }
}
