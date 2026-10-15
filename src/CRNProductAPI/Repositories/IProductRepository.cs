using CRNProductAPI.Models;

namespace CRNProductAPI.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync(int pageNumber, int pageSize);
    Task<Product?> GetByIdAsync(int id);
    Task<Product> AddAsync(Product product);
    Task<bool> UpdateAsync(int id, Product product);
    Task<bool> DeleteAsync(int id);
}
