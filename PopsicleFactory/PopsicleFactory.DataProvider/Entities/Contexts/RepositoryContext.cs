using Microsoft.EntityFrameworkCore;
using PopsicleFactory.DataProvider.Models;

namespace PopsicleFactory.DataProvider.Entities.Contexts;

public class RepositoryContext(DbContextOptions<RepositoryContext> opt) : DbContext(opt)
{
    public DbSet<Popsicle> Popsicles { get; set; }
}
