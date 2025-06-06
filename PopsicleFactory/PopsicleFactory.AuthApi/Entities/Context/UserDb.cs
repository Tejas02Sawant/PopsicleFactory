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
                new User { Id = Guid.NewGuid(), Username= "tejas", Password = "Tejas@12345", Role = "Admin" },
                new User { Id = Guid.NewGuid(), Username= "ryan", Password = "Ryan@12345", Role = "Admin" },
                new User { Id = Guid.NewGuid(), Username= "bob", Password = "Bob@12345", Role = "Admin" },
                new User { Id = Guid.NewGuid(), Username = "jim", Password = "Jim@12345", Role = "Technician" },
                new User { Id = Guid.NewGuid(), Username= "ryan", Password = "Ryan@12345", Role = "Technician" },
                new User { Id = Guid.NewGuid(), Username= "katie", Password = "katie@12345", Role = "Technician" }
            );

            context.SaveChanges();
        } else{
            Console.WriteLine("--> We already have data");
        }
    }
}
