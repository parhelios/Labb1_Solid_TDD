using Microsoft.AspNetCore.Mvc;
using WebShop.DataAccess.UnitOfWork;
using WebShop.Shared.Entities;

namespace WebShop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(IUnitOfWork uow) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
    {
        return Ok(uow.Repository<Order>().GetAllAsync());
    }
}