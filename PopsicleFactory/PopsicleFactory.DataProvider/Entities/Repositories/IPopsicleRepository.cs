using System.Text.Json;
using PopsicleFactory.DataProvider.Models;

namespace PopsicleFactory.DataProvider.Entities.Repositories;

public interface IPopsicleRepository
{
    Task<IEnumerable<Popsicle>> GetAll();
    Task<Popsicle?> GetById(Guid id);
    Task<IEnumerable<Popsicle>> Search(string? flavor, decimal? minPrice, decimal? maxPrice);
    void Create(Popsicle popsicle);
    Task<Popsicle?> Update(Guid id, Popsicle update);
    Task<Popsicle?> PartialUpdate(Guid id, JsonElement updates);
    Task<bool> Delete(Guid id);
    Task<bool> SaveChanges();
}
