using CRNProductAPI.Data;
using CRNProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CRNProductAPI.Services;

public interface IItemService
{
    Task<List<Item>> GetAllAsync(int pageNumber, int pageSize);
    Task<Item?> GetByIdAsync(int id);
    Task<Item?> CreateAsync(Item item);
    Task<bool> UpdateAsync(int id, Item item);
    Task<bool> DeleteAsync(int id);
}
public class ItemService(ApplicationDbContext context) : IItemService
{
    public Task<List<Item>> GetAllAsync(int pageNumber, int pageSize) =>
        context.Items.AsNoTracking().OrderBy(i => i.Id)
            .Skip((Math.Max(1, pageNumber) - 1) * Math.Clamp(pageSize, 1, 100))
            .Take(Math.Clamp(pageSize, 1, 100)).ToListAsync();
    public Task<Item?> GetByIdAsync(int id) => context.Items.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
    public async Task<Item?> CreateAsync(Item item)
    {
        if (!await context.Products.AnyAsync(p => p.Id == item.ProductId)) return null;
        item.Id = 0;
        context.Items.Add(item);
        await context.SaveChangesAsync();
        return item;
    }
    public async Task<bool> UpdateAsync(int id, Item item)
    {
        var existing = await context.Items.FirstOrDefaultAsync(i => i.Id == id);
        if (existing is null || !await context.Products.AnyAsync(p => p.Id == item.ProductId)) return false;
        existing.ProductId = item.ProductId;
        existing.Quantity = item.Quantity;
        await context.SaveChangesAsync();
        return true;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await context.Items.FirstOrDefaultAsync(i => i.Id == id);
        if (existing is null) return false;
        context.Items.Remove(existing);
        await context.SaveChangesAsync();
        return true;
    }
}
