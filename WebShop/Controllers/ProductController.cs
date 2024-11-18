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
        return Ok(uow.ProductRepository.GetAll());
    }

    // Endpoint f�r att l�gga till en ny produkt
    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody]Product product)
    {
        if (product is null)
            return BadRequest("Product is null.");

        try
        {
            uow.ProductRepository.Add(product);
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