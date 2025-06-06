using System.Text.Json;
using PopsicleFactory.DataProvider.Models;

namespace PopsicleFactory.WebApi.Services;

public interface IPopsicleService
{
    Task<IEnumerable<Popsicle>> GetAllPopsicles();
    Task<Popsicle?> GetPopsicleById(Guid id);
    Task<IEnumerable<Popsicle>> Search(string? flavor, decimal? minPrice, decimal? maxPrice);
    Task<Popsicle> CreatePopsicle(Popsicle popsicle);
    Task<Popsicle?> UpdateInformation(Guid id, Popsicle update);
    Task<Popsicle?> PartialUpdate(Guid id, JsonElement updates);
    Task<bool> DeletePopsicle(Guid id);
}
