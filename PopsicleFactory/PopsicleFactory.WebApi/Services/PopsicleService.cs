using System.Text.Json;
using System.Threading.Tasks;
using PopsicleFactory.DataProvider.Entities.Repositories;
using PopsicleFactory.DataProvider.Models;

namespace PopsicleFactory.WebApi.Services;

public class PopsicleService(IPopsicleRepository _repository) : IPopsicleService
{
    public async Task<IEnumerable<Popsicle>> GetAllPopsicles()
    {
        return await _repository.GetAll();
    }

    public async Task<Popsicle?> GetPopsicleById(Guid id)
    {
        return await _repository.GetById(id);
    }

    public async Task<IEnumerable<Popsicle>> Search(string? flavor, decimal? minPrice, decimal? maxPrice)
    {
        return await _repository.Search(flavor, minPrice, maxPrice);
    }

    public async Task<Popsicle> CreatePopsicle(Popsicle popsicle)
    {
        _repository.Create(popsicle);
        await _repository.SaveChanges();

        return popsicle;
    }

    public async Task<Popsicle?> UpdateInformation(Guid id, Popsicle update)
    {
        var popsicle = await _repository.Update(id, update);
        if(popsicle is not null)
            await _repository.SaveChanges();

        return popsicle;
    }

    public async Task<Popsicle?> PartialUpdate(Guid id, JsonElement updates)
    {
        var popsicle = await _repository.PartialUpdate(id, updates);
        if(popsicle is not null)
            await _repository.SaveChanges();

        return popsicle;
    }

    public async Task<bool> DeletePopsicle(Guid id)
    {
        var isDeleted = await _repository.Delete(id);
        if (isDeleted)
            await _repository.SaveChanges();

        return isDeleted;
    }
}
