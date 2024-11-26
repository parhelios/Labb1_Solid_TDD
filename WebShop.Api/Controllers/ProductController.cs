using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebShop.Application.Interfaces;
using WebShop.Domain.Entities;
using WebShop.Infrastructure.Factory;
using WebShop.Infrastructure.Interfaces;
using WebShop.Infrastructure.Observer.Notification;
using WebShop.Infrastructure.Observer;

namespace WebShop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IUnitOfWork uow, ISubjectManager subjectManager) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await uow.Repository<Product>().GetAllAsync();
        if (!products.Any())
            return NotFound(new List<Product>());

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
    public async Task<ActionResult> AddProduct([FromBody] Product product)
    {
        //TODO: Flytta på
        var emailObserver = new EmailNotificationObserver("Test Testies", "test@test.se");

        var subject = subjectManager.Subject<Product>();
        subject.Attach(emailObserver);




        if (!Validator.TryValidateObject(product, new ValidationContext(product), null, true))
            return BadRequest("Invalid product data.");

        if (string.IsNullOrWhiteSpace(product.Name) || product.Price <= 0)
            return BadRequest("Product can't be null or contain whitespace.");

        try
        {
            await uow.Repository<Product>().AddAsync(product);
            await uow.CommitAsync();
            var subjectTest = subjectManager.Subject<Product>();
            subjectTest.Notify(product);
            
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }
        catch (Exception ex)
        {
            uow.Dispose();
            return StatusCode(500, $"Internal server error.");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateProduct(int id, [FromBody] Product product)
    {
        if (!Validator.TryValidateObject(product, new ValidationContext(product), null, true))
            return BadRequest("Invalid product data.");

        if (string.IsNullOrWhiteSpace(product.Name) || product.Price <= 0 || product.Amount < 0)
            return BadRequest("Product can't be null or contain whitespace.");
        
        var productToUpdate = await uow.Repository<Product>().GetByIdAsync(product.Id);
        if (productToUpdate is null)
            return NotFound($"No product with id {product.Id} was found.");

        try
        {
            productToUpdate.Name = product.Name;
            productToUpdate.Price = product.Price;
            productToUpdate.Amount = product.Amount;
            
            await uow.Repository<Product>().UpdateAsync(productToUpdate);
            await uow.CommitAsync();
            return Ok($"Product {product.Id} updated successfully.");
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
            return NotFound($"No product with id {id} was found.");

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

    //[HttpPatch("{id}/amount")]
    //public async Task<IActionResult> UpdateProductAmount(int id, int amount)
    //{
    //    var product = await uow.Repository<Product>().GetByIdAsync(id);
    //    if (product is null)
    //        return NotFound();

    //    try
    //    {
    //        var repo = uow.Repository<Product>() as IProductRepository;            
    //        repo.UpdateProductAmount(id, amount);
    //        await uow.CommitAsync();

    //        return Ok($"Product amount updated to {amount}.");
    //    }
    //    catch (Exception ex)
    //    {
    //        return StatusCode(500, $"Internal server error: {ex.Message}");
    //    }
    //}

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Product>>> SearchProducts([FromQuery] string name)
    {
        var products = await uow.Repository<Product>().FindAsync(p => p.Name.Contains(name));
        if (!products.Any())
            return NotFound(new List<Product>());

        return Ok(products);
    }
}