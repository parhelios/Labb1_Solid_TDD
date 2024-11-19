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
    public async Task GetProducts_ReturnsOkResult_WithAListOfProducts()
    {
        // Arrange
        var products = A.CollectionOfDummy<Product>(5);
        A.CallTo(() => _uow.Repository<Product>()).Returns(_repository);

        // foreach (var prod in products)
        //     await _controller.AddProduct(prod);

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
    public  async Task AddProduct_ReturnsOkResult()
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
}
