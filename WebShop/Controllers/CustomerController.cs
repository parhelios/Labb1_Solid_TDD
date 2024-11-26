using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using WebShop.Application.Interfaces;
using WebShop.Domain.Models;
using WebShop.Infrastructure.Interfaces;

namespace WebShop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController(IUnitOfWork uow) : ControllerBase
{
    private const string EMAILPATTERN = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    private static bool EmailIsValid(string email) => Regex.IsMatch(email, EMAILPATTERN);
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
    {
        var customers = await uow.Repository<Customer>().GetAllAsync();
        if (!customers.Any())
            return NotFound(new List<Customer>());
        
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetCustomerById(int id)
    {
        var customer = await uow.Repository<Customer>().GetByIdAsync(id);
        if (customer is null)
            return NotFound($"No customer with id {id} was found.");
        
        return Ok(customer);
    }

    [HttpPost]
    public async Task<ActionResult> AddCustomer([FromBody] Customer customer)
    {
        if (!Validator.TryValidateObject(customer, new ValidationContext(customer), null, true))
            return BadRequest("Invalid customer data.");

        if (!EmailIsValid(customer.Email))
            return BadRequest("Invalid email address");

        try
        {
            await uow.Repository<Customer>().AddAsync(customer);
            await uow.CommitAsync();

            return CreatedAtAction(nameof(AddCustomer), new { id = customer.Id }, customer);
        }
        catch (Exception ex)
        {
            uow.Dispose();
            return StatusCode(500, $"Internal server error.");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCustomer(int id, [FromBody] Customer customer)
    {
        if (!Validator.TryValidateObject(customer, new ValidationContext(customer), null, true))
            return BadRequest("Invalid customer data.");

        if (string.IsNullOrWhiteSpace(customer.Name) || !EmailIsValid(customer.Email))
            return BadRequest("Invalid customer data format.");
        
        var customerToUpdate = await uow.Repository<Customer>().GetByIdAsync(customer.Id);
        if (customerToUpdate is null)
            return NotFound($"No customer with id {customer.Id} was found.");

        try
        {
            customerToUpdate.Name = customer.Name;
            customerToUpdate.Email = customer.Email;
            
            await uow.Repository<Customer>().UpdateAsync(customerToUpdate);
            await uow.CommitAsync();
            return Ok($"Customer with id {customer.Id} was updated.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCustomer(int id)
    {
        var existingCustomer = await uow.Repository<Customer>().GetByIdAsync(id);
        if (existingCustomer is null)
            return NotFound($"No customer with id {id} was found.");

        await uow.Repository<Customer>().DeleteAsync(id);
        await uow.CommitAsync();

        return Ok();
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Customer>>> SearchCustomers([FromQuery] string name)
    {
        var customers = await uow.Repository<Customer>()
            .FindAsync(c => c.Name.Contains(name));
        return Ok(customers);
    }
}