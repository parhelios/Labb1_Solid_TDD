using Microsoft.AspNetCore.Mvc;
using WebShop.DataAccess.UnitOfWork;
using WebShop.Shared.Entities;

namespace WebShop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IUnitOfWork uow) : ControllerBase
{
    // Endpoint för att hämta alla produkter
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await uow.Repository<Product>().GetAllAsync();

        return Ok(products);
    }

    // Endpoint f�r att l�gga till en ny produkt
    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody]Product product)
    {
        if (product is null)
            return BadRequest("Product is null.");

        try
        {
            await uow.Repository<Product>().AddAsync(product);
            await uow.CommitAsync();
            uow.NotifyProductAdded(product);

            return Ok("Product added successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
            
    }
}