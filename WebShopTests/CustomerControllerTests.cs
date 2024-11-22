using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using WebShop.Controllers;
using WebShop.DataAccess.Factory;
using WebShop.DataAccess.UnitOfWork;
using WebShop.DataAccess;
using WebShop.DataAccess.Repositories;
using WebShop.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace WebShopTests;

public class CustomerControllerTests
{
    private readonly MyDbContext _context;
    private readonly IRepositoryFactory _factory;
    private readonly IUnitOfWork _uow;
    private readonly CustomerController _controller;

    private readonly IUnitOfWork _fakeUow;
    private readonly CustomerController _fakeController;
    private readonly IRepository<Customer> _fakeRepository;

    public CustomerControllerTests()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _context = new MyDbContext(options);
        _factory = new RepositoryFactory(_context);

        _uow = new UnitOfWorkAndRepositoryFactory(_context, _factory);
        _controller = new CustomerController(_uow);

        _fakeUow = A.Fake<IUnitOfWork>();
        _fakeController = new CustomerController(_fakeUow);
        _fakeRepository = A.Fake<IRepository<Customer>>();
    }

    [Fact]
    public async Task AddCustomer_WithValidData_ReturnsOkResult()
    {
        // Arrange
        var customer = new Customer
        {
            Name = "Kurt Kenneth",
            Email = "a@a.a"
        };

        //Act
        var result = await _controller.AddCustomer(customer);

        //Assert
        Assert.IsType<CreatedAtActionResult>(result);
        Assert.Contains(customer, _context.Customers);
        
        await _context.Database.EnsureDeletedAsync();
    }

    [Fact]
    public async Task AddCustomer_WithValidData_ReturnsOkResult_FakeItEasy()
    {
        // Arrange
        var customer = new Customer
        {
            Name = "Kurt Kenneth",
            Email = "a@a.a"
        }; 
        A.CallTo(() => _fakeUow.Repository<Customer>()).Returns(_fakeRepository);
        A.CallTo(() => _fakeRepository.AddAsync(customer)).Returns(Task.CompletedTask);

        //Act
        var result = await _fakeController.AddCustomer(customer);

        //Assert
        Assert.IsType<CreatedAtActionResult>(result);
        A.CallTo(() => _fakeUow.Repository<Customer>()).MustHaveHappenedOnceExactly();
        A.CallTo(() => _fakeRepository.AddAsync(customer)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task AddCustomer_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange
        var customer = new Customer
        {
            Name = "Kurt Kenneth",
            Email = "a"
        };

        //_controller.ModelState.AddModelError("Email", "Invalid email");

        //Act
        var result = await _controller.AddCustomer(customer);

        //Assert
        Assert.IsType<BadRequestObjectResult>(result);
        await _context.Database.EnsureDeletedAsync();
    }

    [Fact]
    public async Task UpdateCustomer_WithValidData_ReturnsOkResult()
    {
        // Arrange
        var customer = new Customer
        {
            Id = 1,
            Name = "Kurt Kenneth",
            Email = "a@a.a"
        };

        await _controller.AddCustomer(customer);

        customer.Name = "Kurt Kenneth Jr.";

        //Act
        var result = await _controller.UpdateCustomer(1, customer);

        //Assert
        Assert.IsType<OkResult>(result);
        var updatedUser = _context.Customers.Find(1);
        Assert.Equal("Kurt Kenneth Jr.", updatedUser.Name);
        
        await _context.Database.EnsureDeletedAsync();
    }
}