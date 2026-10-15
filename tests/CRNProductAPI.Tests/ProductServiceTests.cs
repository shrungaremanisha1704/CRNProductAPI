using CRNProductAPI.Models;
using CRNProductAPI.Repositories;
using CRNProductAPI.Services;
using Moq;
using Xunit;

namespace CRNProductAPI.Tests;

public class ProductServiceTests
{
    [Fact]
    public async Task GetAllProducts_ClampsPageSizeToMaximum100()
    {
        var repository = new Mock<IProductRepository>();
        repository.Setup(r => r.GetAllAsync(2, 100)).ReturnsAsync(new List<Product>());
        var service = new ProductService(repository.Object);

        var result = await service.GetAllProductsAsync(2, 500);

        Assert.Empty(result);
        repository.Verify(r => r.GetAllAsync(2, 100), Times.Once);
    }

    [Fact]
    public async Task CreateProduct_SetsAuditFieldsAndTrimsText()
    {
        var repository = new Mock<IProductRepository>();
        repository.Setup(r => r.AddAsync(It.IsAny<Product>()))
            .ReturnsAsync((Product p) => { p.Id = 10; return p; });
        var service = new ProductService(repository.Object);

        var result = await service.CreateProductAsync(new Product
        {
            Id = 99, ProductName = "  Keyboard  ", CreatedBy = "  Tester  ",
            ModifiedBy = "old", ModifiedOn = DateTime.UtcNow
        });

        Assert.Equal(10, result.Id);
        Assert.Equal("Keyboard", result.ProductName);
        Assert.Equal("Tester", result.CreatedBy);
        Assert.Null(result.ModifiedBy);
        Assert.Null(result.ModifiedOn);
        Assert.True(result.CreatedOn > DateTime.UnixEpoch);
    }

    [Fact]
    public async Task DeleteProduct_DelegatesToRepository()
    {
        var repository = new Mock<IProductRepository>();
        repository.Setup(r => r.DeleteAsync(4)).ReturnsAsync(true);
        var service = new ProductService(repository.Object);

        Assert.True(await service.DeleteProductAsync(4));
        repository.Verify(r => r.DeleteAsync(4), Times.Once);
    }
}
