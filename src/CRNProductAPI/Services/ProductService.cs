using CRNProductAPI.Models;
using CRNProductAPI.Repositories;

namespace CRNProductAPI.Services;

public class ProductService(IProductRepository repository) : IProductService
{
    public Task<List<Product>> GetAllProductsAsync(int pageNumber, int pageSize) =>
        repository.GetAllAsync(Math.Max(1, pageNumber), Math.Clamp(pageSize, 1, 100));

    public Task<Product?> GetProductByIdAsync(int id) => repository.GetByIdAsync(id);

    public Task<Product> CreateProductAsync(Product product)
    {
        product.Id = 0;
        product.ProductName = product.ProductName.Trim();
        product.CreatedBy = product.CreatedBy.Trim();
        product.CreatedOn = DateTime.UtcNow;
        product.ModifiedBy = null;
        product.ModifiedOn = null;
        return repository.AddAsync(product);
    }

    public Task<bool> UpdateProductAsync(int id, Product product)
    {
        product.ProductName = product.ProductName.Trim();
        product.ModifiedBy = product.ModifiedBy?.Trim();
        return repository.UpdateAsync(id, product);
    }

    public Task<bool> DeleteProductAsync(int id) => repository.DeleteAsync(id);
}
