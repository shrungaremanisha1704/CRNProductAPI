using CRNProductAPI.Models;
using CRNProductAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRNProductAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public class ProductsController(IProductService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
            return BadRequest(new { message = "pageNumber must be >= 1 and pageSize must be between 1 and 100." });
        return Ok(await service.GetAllProductsAsync(pageNumber, pageSize));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id < 1) return BadRequest(new { message = "Id must be greater than zero." });
        var product = await service.GetProductByIdAsync(id);
        return product is null ? NotFound(new { message = "Product not found." }) : Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductRequest request)
    {
        var product = new Product { ProductName = request.ProductName, CreatedBy = request.CreatedBy };
        var created = await service.CreateProductAsync(product);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateProductRequest request)
    {
        if (id < 1) return BadRequest(new { message = "Id must be greater than zero." });
        var ok = await service.UpdateProductAsync(id, new Product { ProductName = request.ProductName, ModifiedBy = request.ModifiedBy });
        return ok ? NoContent() : NotFound(new { message = "Product not found." });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id < 1) return BadRequest(new { message = "Id must be greater than zero." });
        return await service.DeleteProductAsync(id) ? NoContent() : NotFound(new { message = "Product not found." });
    }
}
