using Microsoft.EntityFrameworkCore;
using WebShop.Controllers;
using WebShop.DataAccess.Factory;
using WebShop.DataAccess.UnitOfWork;
using WebShop.DataAccess;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using WebShop.Shared.Models;

namespace WebShopTests;

public class RepositoryTests
{
    private readonly MyDbContext _context;
    private readonly IRepositoryFactory _factory;
    private readonly IUnitOfWork _uow;

    public RepositoryTests()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _context = new MyDbContext(options);
        _factory = new RepositoryFactory(_context);

        _uow = new UnitOfWork(_context, _factory);
    }
    public async Task AddProduct_ReturnsOkResult()
    {
        // Arrange
        var product = new Product
        {
            Name = "Test Product",
            Price = 10,
            Amount = 5
        };

        // Act
        await _uow.Repository<Product>().AddAsync(product);

        // Assert
        //Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetProductById_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var sut = _uow.Repository<Product>();
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Price = 10,
            Amount = 5
        };
        await sut.AddAsync(product);

        // Act
        var result = await sut.GetByIdAsync(2);

        // Assert
        Assert.Equal(null, result);
    }

}