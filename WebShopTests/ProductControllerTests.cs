using FakeItEasy;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebShop.Controllers;
using WebShop.DataAccess.Repositories;
using WebShop.DataAccess.UnitOfWork;
using WebShop.Shared.Models;

namespace WebShopTests;

public class ProductControllerTests
{
    //TODO: Skriv om tester i FakeItEasy

    private readonly IUnitOfWork _uow;
    private readonly ProductController _controller;
    private readonly IRepository<Product> _repository;

    public ProductControllerTests()
    {
        _uow = A.Fake<IUnitOfWork>();
        _controller = new ProductController(_uow);
        _repository = A.Fake<IRepository<Product>>();
    }
    
    [Fact]
    public async Task AddProduct_ReturnsOkResult()
    {
        // Arrange
        var product = A.Dummy<Product>();
        A.CallTo(() => _uow.Repository<Product>()).Returns(_repository);

        // Act
        var result = await _controller.AddProduct(product);

        // Assert
        Assert.IsType<OkObjectResult>(result);

        A.CallTo(() => _repository.AddAsync(product)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _uow.CommitAsync()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetProducts_ReturnsOkResult_WithAListOfProducts()
    {
        // Arrange
        var products = A.CollectionOfDummy<Product>(5);
        A.CallTo(() => _repository.GetAllAsync()).Returns(Task.FromResult((IEnumerable<Product>)products));
        A.CallTo(() => _uow.Repository<Product>()).Returns(_repository);

        // foreach (var prod in products)
        //     // await _repository.AddAsync(prod);
        //     // await _uow.Repository<Product>().AddAsync(prod);
        //     await _controller.AddProduct(prod);
        // await _uow.CommitAsync();

        // Act
        var result = await _controller.GetProducts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
        Assert.Equal(products, returnedProducts);
        Assert.Equal(5, returnedProducts.Count());
        A.CallTo(() => _uow.Repository<Product>().GetAllAsync()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetProductById_ReturnsOkResult_WithAProduct()
    {
        // Arrange
        var product = A.Dummy<Product>();
        A.CallTo(() => _repository.GetByIdAsync(product.Id)).Returns(Task.FromResult(product));
        // A.CallTo(() => _uow.Repository<Product>().GetByIdAsync(product.Id)).Returns(Task.FromResult(product));
        A.CallTo(() => _uow.Repository<Product>()).Returns(_repository);
        // await _controller.AddProduct(product);

        // Act
        var result = await _controller.GetProductById(product.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProduct = Assert.IsAssignableFrom<Product>(okResult.Value);
        Assert.Equal(product, returnedProduct);
        A.CallTo(() => _uow.Repository<Product>().GetByIdAsync(product.Id)).MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task UpdateProduct_ReturnsOkResult()
    {
        // Arrange
        var product = A.Dummy<Product>();
        A.CallTo(() => _repository.GetByIdAsync(product.Id)).Returns(Task.FromResult(product));
        A.CallTo(() => _uow.Repository<Product>()).Returns(_repository);

        // Act
        var result = await _controller.UpdateProduct(product.Id, product);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        A.CallTo(() => _repository.UpdateAsync(product)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _uow.CommitAsync()).MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task DeleteProduct_ReturnsOkResult()
    {
        // Arrange
        var product = A.Dummy<Product>();
        A.CallTo(() => _repository.GetByIdAsync(product.Id)).Returns(Task.FromResult(product));
        A.CallTo(() => _uow.Repository<Product>()).Returns(_repository);

        // Act
        var result = await _controller.DeleteProduct(product.Id);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        A.CallTo(() => _repository.DeleteAsync(product.Id)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _uow.CommitAsync()).MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task DeleteProduct_ReturnsNotFoundResult()
    {
        // Arrange
        var product = A.Dummy<Product>();
        A.CallTo(() => _repository.GetByIdAsync(product.Id)).Returns(Task.FromResult((Product)null));
        A.CallTo(() => _uow.Repository<Product>()).Returns(_repository);

        // Act
        var result = await _controller.DeleteProduct(product.Id);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        A.CallTo(() => _repository.DeleteAsync(product.Id)).MustNotHaveHappened();
        A.CallTo(() => _uow.CommitAsync()).MustNotHaveHappened();
    }
    
    [Fact]
    public async Task UpdateProductAmount_ReturnsOkResult()
    {
        // Arrange
        var product = A.Dummy<Product>();
        // A.CallTo(() => _repository.GetByIdAsync(product.Id)).Returns(Task.FromResult(product));
        // A.CallTo(() => _uow.Repository<Product>()).Returns(_repository);

        // Act
        var result = await _controller.UpdateProductAmount(product.Id, 10);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        // A.CallTo(() => _repository.UpdateAsync(product)).MustHaveHappenedOnceExactly();
        // A.CallTo(() => _uow.CommitAsync()).MustHaveHappenedOnceExactly();
    }
}
