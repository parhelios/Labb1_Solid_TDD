using Microsoft.AspNetCore.Mvc;
using WebShop.DataAccess.Repositories;
using WebShop.DataAccess.UnitOfWork;
using WebShop.Shared.Models;

namespace WebShop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IUnitOfWork uow) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await uow.Repository<Product>().GetAllAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProductById(int id)
    {
        var product = await uow.Repository<Product>().GetByIdAsync(id);
        if (product is null)
            return NotFound($"No product with id {id} was found.");

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult> AddProduct([FromBody]Product product)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await uow.Repository<Product>().AddAsync(product);
            await uow.CommitAsync();
            uow.NotifyProductAdded(product);

            return Ok("Product added successfully.");
        }
        catch (Exception ex)
        {
            uow.Dispose();
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
            
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id != product.Id)
            return BadRequest("ID mismatch.");

        try
        {
            await uow.Repository<Product>().UpdateAsync(product);
            await uow.CommitAsync();

            return Ok($"Product {id} updated successfully.");
        }
        catch (Exception ex)
        {
            uow.Dispose();
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var existingProduct = await uow.Repository<Product>().GetByIdAsync(id);
        if (existingProduct is null)
            return NotFound();

        try
        {
            await uow.Repository<Product>().DeleteAsync(id);
            await uow.CommitAsync();

            return Ok($"Product {existingProduct.Name} deleted successfully.");
        }
        catch (Exception ex)
        {
            uow.Dispose();
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPatch("{id}/amount")]
    public async Task<IActionResult> UpdateProductAmount(int id, int amount)
    {
        var product = await uow.Repository<Product>().GetByIdAsync(id);
        if (product is null)
            return NotFound();

        try
        {
            var repo = uow.Repository<Product>() as IProductRepository;            
            repo.UpdateProductAmount(id, amount);
            await uow.CommitAsync();

            return Ok($"Product amount updated to {amount}.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Product>>> SearchProducts([FromQuery] string name)
    {
        var products = await uow.Repository<Product>().FindAsync(p => p.Name.Contains(name));
        return Ok(products);
    }
}