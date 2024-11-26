using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShop.Controllers;
using WebShop.Domain.Entities;
using WebShop.Infrastructure.DataAccess;
using WebShop.Infrastructure.Factory;
using WebShop.Infrastructure.Interfaces;
using WebShop.Infrastructure.UnitOfWork;

namespace WebShop.Tests;

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

        _uow = new UnitOfWork(_context, _factory);
        _controller = new CustomerController(_uow);

        _fakeUow = A.Fake<IUnitOfWork>();
        _fakeController = new CustomerController(_fakeUow);
        _fakeRepository = A.Fake<IRepository<Customer>>();
    }

    [Fact]
    public async Task AddCustomer_WithValidData_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var customer = new Customer
        {
            Name = "Kurt Kenneth",
            Email = "a@a.a"
        };
        A.CallTo(() => _fakeRepository.AddAsync(customer)).Returns(Task.CompletedTask);

        //Act
        var result = await _fakeController.AddCustomer(customer);

        //Assert
        Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(customer, ((CreatedAtActionResult)result).Value);
        
        A.CallTo(() => _fakeUow.CommitAsync()).MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task AddCustomer_WithInvalidEmail_ReturnsBadRequest()
    {
        // Arrange
        var customer = new Customer
        {
            Name = "Kurt Kenneth",
            Email = "a"
        };
        
        //Act
        var result = await _controller.AddCustomer(customer);

        //Assert
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, ((BadRequestObjectResult)result).StatusCode);
        Assert.Equal("Invalid customer data.", ((BadRequestObjectResult)result).Value);

        await _context.Database.EnsureDeletedAsync();
    }
    
    [Fact]
    public async Task AddCustomer_WhenRepositoryThrowsException_ReturnsInternalServerError()
    {
        // Arrange
        var customer = new Customer
        {
            Name = "Kurt Kenneth",
            Email = "a@a.a"
        };
        A.CallTo(() => _fakeRepository.AddAsync(customer)).Throws(new Exception());
        A.CallTo(() => _fakeUow.Repository<Customer>()).Returns(_fakeRepository);
        
        //Act
        var result = await _fakeController.AddCustomer(customer);

        //Assert
        var objectResult = Assert.IsType<ObjectResult>(result);

        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Internal server error.", objectResult.Value);

        A.CallTo(() => _fakeUow.Dispose()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    private async Task GetAllCustomers_ReturnsOkResult_WithListOfCustomers()
    {
        //Arrange
        var customers = A.CollectionOfDummy<Customer>(5);
        
        A.CallTo(() => _fakeRepository.GetAllAsync()).Returns(customers);
        A.CallTo(() => _fakeUow.Repository<Customer>()).Returns(_fakeRepository);
        
        //Act
        var result = await _fakeController.GetCustomers();

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);  
        var returnCustomers = Assert.IsAssignableFrom<IEnumerable<Customer>>(okResult.Value);  
        Assert.Equal(customers.Count, returnCustomers.Count());

        A.CallTo(() => _fakeRepository.GetAllAsync()).MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task GetCustomers_WithNoCustomersInDb_ReturnsOkResult_WithEmptyList()
    {
        // Arrange
        A.CallTo(() => _fakeRepository.GetAllAsync()).Returns(Task.FromResult((IEnumerable<Customer>)new List<Customer>()));
        A.CallTo(() => _fakeUow.Repository<Customer>()).Returns(_fakeRepository);

        // Act
        var result = await _fakeController.GetCustomers();

        // Assert
        var okResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        var returnedCustomers = Assert.IsAssignableFrom<IEnumerable<Customer>>(okResult.Value);
        Assert.Empty(returnedCustomers);
        A.CallTo(() => _fakeRepository.GetAllAsync()).MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task GetCustomerById_WithValidId_ReturnsOkResultWithACustomer()
    {
        // Arrange
        var customer = A.Dummy<Customer>();
        A.CallTo(() => _fakeRepository.GetByIdAsync(customer.Id)).Returns(Task.FromResult(customer));
        A.CallTo(() => _fakeUow.Repository<Customer>()).Returns(_fakeRepository);

        // Act
        var result = await _fakeController.GetCustomerById(customer.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCustomer = Assert.IsAssignableFrom<Customer>(okResult.Value);
        Assert.Equal(customer, returnedCustomer);
        A.CallTo(() => _fakeUow.Repository<Customer>().GetByIdAsync(customer.Id)).MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task GetCustomerById_WithInvalidId_ReturnsNotFoundResult()
    {
        // Arrange
        var customer = new Customer
        {
            Name = "Kurt Kenneth",
            Email = "a@a.a"
        };
        await _controller.AddCustomer(customer);

        // Act
        var result = await _controller.GetCustomerById(999);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
        
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
        Assert.IsType<OkObjectResult>(result);
        var updatedUser = _context.Customers.Find(1);
        Assert.Equal("Kurt Kenneth Jr.", updatedUser.Name);
        
        await _context.Database.EnsureDeletedAsync();
    }
}