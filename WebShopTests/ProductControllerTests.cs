using Moq;
using WebShop.Controllers;
using WebShop.DataAccess.Repositories;

namespace WebShopTests;

public class ProductControllerTests
{
    //TODO: Skriv om tester i FakeItEasy
    
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly ProductController _controller;

    public ProductControllerTests()
    {
        _mockProductRepository = new Mock<IProductRepository>();

        // St�ll in mock av Products-egenskapen
    }

    [Fact]
    public void GetProducts_ReturnsOkResult_WithAListOfProducts()
    {
        // Arrange

        // Act

        // Assert
    }

    [Fact]
    public void AddProduct_ReturnsOkResult()
    {
        // Arrange

        // Act

        // Assert
    }
}