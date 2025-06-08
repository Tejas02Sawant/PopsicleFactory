using Microsoft.EntityFrameworkCore;

namespace PopsicleFactory.AuthApi.Entities.Context;

public class UserContext(DbContextOptions<UserContext> opt) : DbContext(opt)
{
    public DbSet<User> Users { get; set; }
}
