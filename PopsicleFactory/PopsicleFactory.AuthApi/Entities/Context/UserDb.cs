using PopsicleFactory.AuthApi.Entities.Context;

namespace PopsicleFactory.AuthApi.Entities;

public static class UserDb
{
    public static void PrepPopulation(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateAsyncScope();
        SeedData(serviceScope.ServiceProvider.GetService<UserContext>()!);
    }

    private static void SeedData(UserContext context)
    {
        if(!context.Users.Any())
        {
            Console.WriteLine("--> Seeding data...");

            context.Users.AddRange(
                new User { Id = Guid.NewGuid(), Username= "tejas@demo.com", Password = "Tejas@12345", Role = "Admin" },
                new User { Id = Guid.NewGuid(), Username= "ryan@demo.com", Password = "Ryan@12345", Role = "Admin" },
                new User { Id = Guid.NewGuid(), Username= "katie@demo.com", Password = "Katie@12345", Role = "Admin" },
                new User { Id = Guid.NewGuid(), Username= "bob@demo.com", Password = "Bob@12345", Role = "User" },
                new User { Id = Guid.NewGuid(), Username= "maria@demo.com", Password = "Maria@12345", Role = "User" }
            );

            context.SaveChanges();
        } else{
            Console.WriteLine("--> We already have data");
        }
    }
}
