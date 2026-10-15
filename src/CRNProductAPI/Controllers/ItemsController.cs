using CRNProductAPI.Models;
using CRNProductAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRNProductAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public class ItemsController(IItemService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
            return BadRequest(new { message = "pageNumber must be >= 1 and pageSize must be between 1 and 100." });
        return Ok(await service.GetAllAsync(pageNumber, pageSize));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id < 1) return BadRequest(new { message = "Id must be greater than zero." });
        var item = await service.GetByIdAsync(id);
        return item is null ? NotFound(new { message = "Item not found." }) : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateItemRequest request)
    {
        var created = await service.CreateAsync(new Item { ProductId = request.ProductId, Quantity = request.Quantity });
        return created is null ? BadRequest(new { message = "ProductId does not exist." }) : CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateItemRequest request)
    {
        if (id < 1) return BadRequest(new { message = "Id must be greater than zero." });
        var ok = await service.UpdateAsync(id, new Item { ProductId = request.ProductId, Quantity = request.Quantity });
        return ok ? NoContent() : NotFound(new { message = "Item or referenced product not found." });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id < 1) return BadRequest(new { message = "Id must be greater than zero." });
        return await service.DeleteAsync(id) ? NoContent() : NotFound(new { message = "Item not found." });
    }
}
