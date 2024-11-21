using Microsoft.EntityFrameworkCore;
using WebShop.Controllers;
using WebShop.DataAccess.Factory;
using WebShop.DataAccess.UnitOfWork;
using WebShop.DataAccess;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using WebShop.DataAccess.Repositories;
using WebShop.Shared.Models;
using WebShopTests.TestData;

namespace WebShopTests;

public class RepositoryTests
{
    private readonly MyDbContext _dbContext;
    private readonly IRepository<Product> _repository;

    public RepositoryTests()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;
        
        _dbContext = new MyDbContext(options);
        _repository = new Repository<Product>(_dbContext);

    }
    
    [Fact]
    public async Task AddAsync_WithValidData_ShouldAddEntityToDbSet()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Product Add Test",
            Price = 11,
            Amount = 5
        };

        // Act
        await _repository.AddAsync(product);
        await _dbContext.SaveChangesAsync();

        // Assert
        var addedProduct = await _dbContext.Products.FindAsync(1);
        Assert.NotNull(addedProduct);
        Assert.Equal("Product Add Test", addedProduct.Name);
        
        await _dbContext.Database.EnsureDeletedAsync();
    }

    [Fact]
    public async Task GetByIdAsync_WithValidData_ShouldReturnProductInDb()
    {
        //Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Product Add Test",
            Price = 11,
            Amount = 5
        };
        
        await _repository.AddAsync(product);
        await _dbContext.SaveChangesAsync();
        
        //Act
        var productInDb = await _dbContext.Products.FindAsync(1);
        
        //Asser
        Assert.NotNull(productInDb);
        Assert.Equal("Product Add Test", productInDb.Name);
        
        await _dbContext.Database.EnsureDeletedAsync();
    }

    [Theory]
    [ClassData(typeof(ProductTestData))]
    public async Task GetAllAsync_WithValidData_ShouldReturnAllProductsInDb(object input)
    {
        //Arrange
        var products = input switch
        {
            Product singleProduct => new List<Product> { singleProduct },
            Product[] productArray => new List<Product>(productArray),
            _ => throw new ArgumentException("Invalid input type")
        };
        
        foreach (var p in products)
            await _repository.AddAsync(p);
        await _dbContext.SaveChangesAsync();

        //Act
        var result = await _repository.GetAllAsync();
        
        //Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(products.Count, result.Count());
        
        await _dbContext.Database.EnsureDeletedAsync();
    }

    [Fact]
    public async Task UpdateAsync_WithValidData_ShouldUpdateEntityInDb()
    {
        //Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Product Add Test",
            Price = 11,
            Amount = 5
        };
        
        await _repository.AddAsync(product);
        await _dbContext.SaveChangesAsync();
        product.Name = "Product Updated Test";
        
        //Act
        await _repository.UpdateAsync(product);
        await _dbContext.SaveChangesAsync();

        var result = await _dbContext.Products.FindAsync(1);
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal("Product Updated Test", result.Name);
        
        await _dbContext.Database.EnsureDeletedAsync();
    }

    [Fact]
    public async Task DeleteAsync_WithValidData_ShouldDeleteProductInDb()
    {
        //Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Product Add Test",
            Price = 11,
            Amount = 5
        };
        
        await _repository.AddAsync(product);
        await _dbContext.SaveChangesAsync();
        var addedProduct = await _dbContext.Products.FindAsync(1);
        
        //Act
        await _repository.DeleteAsync(1);
        await _dbContext.SaveChangesAsync();
        
        //Assert: Ensure that product with ID 1 was added to database, and that it no longer exists there.
        Assert.NotNull(addedProduct);
        var result = await _dbContext.Products.FindAsync(1);
        Assert.Null(result);
        
        await _dbContext.Database.EnsureDeletedAsync();
    }
    
    [Fact]
    public async Task DeleteAsync_WithInvalidId_ShouldReturnNull()
    {
        //Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Product Add Test",
            Price = 11,
            Amount = 5
        };
        
        await _repository.AddAsync(product);
        await _dbContext.SaveChangesAsync();
        var addedProduct = await _dbContext.Products.FindAsync(1);
        
        //Act: Try to delete a product that doesn't exist (ID = 11)
        await _repository.DeleteAsync(11);
        await _dbContext.SaveChangesAsync();
        
        //Assert: Ensure the product with ID 1 is still present in the database
        var result = await _dbContext.Products.FindAsync(1);
        Assert.NotNull(result);
        
        var allProducts = await _dbContext.Products.ToListAsync();
        Assert.Single(allProducts);
        
        await _dbContext.Database.EnsureDeletedAsync();
    }

    [Theory]
    [ClassData(typeof(ProductTestData))]
    public async Task TaskFindAsyndicate_WithValidId_ShouldReturnCorrectProduct(object input)
    {
        //Arrange
        var products = input switch
        {
            Product singleProduct => new List<Product> { singleProduct },
            Product[] productArray => new List<Product>(productArray),
            _ => throw new ArgumentException("Invalid input type")
        };
        
        foreach (var p in products)
            await _repository.AddAsync(p);
        await _dbContext.SaveChangesAsync();
        
        //Act
        var result = await _dbContext.Products.FindAsync(products[0].Id);
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal(products[0].Id, result.Id); 
        Assert.Equal(products[0].Name, result.Name); 
        Assert.Equal(products[0].Price, result.Price); 
        Assert.Equal(products[0].Amount, result.Amount); 
        
        await _dbContext.Database.EnsureDeletedAsync();
    }
}