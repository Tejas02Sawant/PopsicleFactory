using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PopsicleFactory.DataProvider.Entities.Contexts;
using PopsicleFactory.DataProvider.Models;

namespace PopsicleFactory.DataProvider.Entities.Repositories;

public class PopsicleRepository(RepositoryContext _context) : IPopsicleRepository
{
    public async Task<IEnumerable<Popsicle>> GetAll()
    {
        return await _context.Popsicles.AsNoTracking().ToListAsync();
    }

    public async Task<Popsicle?> GetById(Guid id)
    {
        return await _context.Popsicles.FirstOrDefaultAsync(p=> p.Id == id);
    }

    public async Task<IEnumerable<Popsicle>> Search(string? flavor, decimal? minPrice, decimal? maxPrice)
    {
        var query = _context.Popsicles.AsQueryable();

        if (!string.IsNullOrEmpty(flavor))
            query = query.Where(p => p.Flavor.Contains(flavor));

        if (minPrice.HasValue)
            query = query.Where(p => p.Price >= minPrice);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice);

        return await query.ToListAsync();
    }

    public async void Create(Popsicle popsicle)
    {
        await _context.Popsicles.AddAsync(popsicle);
    }

    public async Task<Popsicle?> PartialUpdate(Guid id, JsonElement items)
    {
        var popsicle = await _context.Popsicles.FindAsync(id);
        if (popsicle == null) return null;

        foreach (var prop in items.EnumerateObject())
        {
            switch (prop.Name.ToLower())
            {
                case "flavor":
                    popsicle.Flavor = prop.Value.GetString() ?? popsicle.Flavor;
                    break;
                case "quantity":
                    popsicle.Quantity = prop.Value.GetInt32();
                    break;
                case "price":
                    popsicle.Price = prop.Value.GetDecimal();
                    break;
            }
        }

        return popsicle;
    }

    public async Task<Popsicle?> Update(Guid id, Popsicle item)
    {
        var existing = await _context.Popsicles.FindAsync(id);
        if (existing == null) return null;

        existing.Flavor = item.Flavor;
        existing.Quantity = item.Quantity;
        existing.Price = item.Price;

        return existing;
    }

    public async Task<bool> Delete(Guid id)
    {
        var popsicle = await _context.Popsicles.FindAsync(id);
        if (popsicle == null) return false;

        _context.Popsicles.Remove(popsicle);
        return true;
    }

    public async Task<bool> SaveChanges() => await _context.SaveChangesAsync() >= 0;
}
