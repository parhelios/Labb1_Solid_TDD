﻿using Microsoft.AspNetCore.Mvc;
using WebShop.DataAccess.Repositories;
using WebShop.DataAccess.UnitOfWork;
using WebShop.Shared.Entities;

namespace WebShop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController(IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
    {
        return Ok(await unitOfWork.CustomerRepository.GetAll());
    }
}