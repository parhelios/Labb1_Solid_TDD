using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using WebShop.DataAccess.UnitOfWork;
using WebShop.Shared.Models;

namespace WebShop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController(IUnitOfWork uow) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
    {
        var customer = await uow.Repository<Customer>().GetAllAsync();
        return Ok(customer);
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
        //if (!ModelState.IsValid)
        //    return BadRequest(ModelState);

        const string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        if (!Regex.IsMatch(customer.Email, emailPattern))
            return BadRequest("Invalid email address");

        await uow.Repository<Customer>().AddAsync(customer);
        await uow.CommitAsync();

        return CreatedAtAction(nameof(AddCustomer), new { id = customer.Id }, customer);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCustomer(int id, [FromBody] Customer customer)
    {
        //if (!ModelState.IsValid)
        //    return BadRequest(ModelState);

        if (id != customer.Id)
            return BadRequest("ID mismatch");

        await uow.Repository<Customer>().UpdateAsync(customer);
        await uow.CommitAsync();

        return Ok();
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