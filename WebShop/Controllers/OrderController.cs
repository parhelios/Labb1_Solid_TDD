using Microsoft.AspNetCore.Mvc;
using WebShop.DataAccess.UnitOfWork;
using WebShop.Shared.Models;

namespace WebShop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(IUnitOfWork uow) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
    {
        var order = await uow.Repository<Order>().GetAllAsync();
        return Ok(order);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrderById(int id)
    {
        var order = await uow.Repository<Order>().GetByIdAsync(id);
        if (order is null)
            return NotFound($"No order with id {id} was found.");

        return Ok(order);
    }

    [HttpPost]
    public async Task<ActionResult> AddOrder([FromBody] Order order)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await uow.Repository<Order>().AddAsync(order);
        await uow.CommitAsync();

        return CreatedAtAction(nameof(AddOrder), new { id = order.Id }, order);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateOrder(int id, [FromBody] Order order)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id != order.Id)
            return BadRequest("ID mismatch");

        await uow.Repository<Order>().UpdateAsync(order);
        await uow.CommitAsync();

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOrder(int id)
    {
        var existingOrder = await uow.Repository<Order>().GetByIdAsync(id);
        if (existingOrder is null)
            return NotFound($"No order with id {id} was found.");

        await uow.Repository<Order>().DeleteAsync(id);
        await uow.CommitAsync();

        return Ok();
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Order>>> SearchOrders([FromQuery] DateTime date)
    {
        //TODO: Implement search logic + fix OrderDate
        //var orders = await uow.Repository<Order>().FindAsync(o => o.OrderDate == date);
        //return Ok(orders);

        return null;
    }
}