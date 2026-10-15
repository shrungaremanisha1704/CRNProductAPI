using CRNProductAPI.Models;

namespace CRNProductAPI.Services;

public interface IProductService
{
    Task<List<Product>> GetAllProductsAsync(int pageNumber, int pageSize);
    Task<Product?> GetProductByIdAsync(int id);
    Task<Product> CreateProductAsync(Product product);
    Task<bool> UpdateProductAsync(int id, Product product);
    Task<bool> DeleteProductAsync(int id);
}
