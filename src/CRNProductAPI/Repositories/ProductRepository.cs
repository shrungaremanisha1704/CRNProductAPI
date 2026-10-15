using CRNProductAPI.Data;
using CRNProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CRNProductAPI.Repositories;

public class ProductRepository(ApplicationDbContext context) : IProductRepository
{
    public Task<List<Product>> GetAllAsync(int pageNumber, int pageSize) =>
        context.Products.AsNoTracking().OrderBy(p => p.Id)
            .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

    public Task<Product?> GetByIdAsync(int id) =>
        context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Product> AddAsync(Product product)
    {
        context.Products.Add(product);
        await context.SaveChangesAsync();
        return product;
    }

    public async Task<bool> UpdateAsync(int id, Product product)
    {
        var existing = await context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (existing is null) return false;
        existing.ProductName = product.ProductName;
        existing.ModifiedBy = product.ModifiedBy;
        existing.ModifiedOn = DateTime.UtcNow;
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (existing is null) return false;
        context.Products.Remove(existing);
        await context.SaveChangesAsync();
        return true;
    }
}
