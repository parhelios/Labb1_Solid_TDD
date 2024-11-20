using System.Collections;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShop.Controllers;
using WebShop.DataAccess;
using WebShop.DataAccess.Factory;
using WebShop.DataAccess.Repositories;
using WebShop.DataAccess.UnitOfWork;
using WebShop.Shared.Models;

namespace WebShopTests;

public class ProductControllerTests
{
    private readonly MyDbContext _context;
    private readonly IRepositoryFactory _factory;
    private readonly IUnitOfWork _uow;
    private readonly ProductController _controller;

    private readonly IUnitOfWork _fakeUow;
    private readonly ProductController _fakeController;
    private readonly IRepository<Product> _fakeRepository;

    public ProductControllerTests()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _context = new MyDbContext(options);
        _factory = new RepositoryFactory(_context);

        _uow = new UnitOfWork(_context, _factory);
        _controller = new ProductController(_uow);

        _fakeUow = A.Fake<IUnitOfWork>();
        _fakeController = new ProductController(_fakeUow);
        _fakeRepository = A.Fake<IRepository<Product>>();
    }

    [Fact]
    public async Task AddProduct_ReturnsOkResult()
    {
        // Arrange
        //var product = A.Dummy<Product>();
        var product = new Product
        {
            Name = "Test Product",
            Price = 10,
            Amount = 5
        };
        A.CallTo(() => _fakeUow.Repository<Product>()).Returns(_fakeRepository);

        // Act
        var result = await _fakeController.AddProduct(product);

        // Assert
        Assert.IsType<OkObjectResult>(result);

        A.CallTo(() => _fakeRepository.AddAsync(product)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _fakeUow.CommitAsync()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task AddProduct_WithInvalidData_ReturnsBadRequestResult()
    {
        // Arrange
        var product = new Product
        {
            Name = string.Empty,
            Price = -1,
            Amount = 5
        };
        _controller.ModelState.AddModelError("Name", "Name is required.");

        // Act
        var result = await _controller.AddProduct(product);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task AddProduct_WithInvalidInput_ReturnsBadRequest()
    {
        // Arrange
        var product = new Product
        {
            Name = "T",
            Amount = 20,
            Price = 30
        };

        // Act
        var result = await _controller.AddProduct(product);

        // Assert
        Assert.IsAssignableFrom<BadRequestResult>(result);
    }

    [Fact]
    public async Task GetProducts_ReturnsOkResult_WithAListOfProducts()
    {
        // Arrange
        var products = A.CollectionOfDummy<Product>(5);
        A.CallTo(() => _fakeRepository.GetAllAsync()).Returns(Task.FromResult((IEnumerable<Product>)products));
        A.CallTo(() => _fakeUow.Repository<Product>()).Returns(_fakeRepository);

        // Act
        var result = await _fakeController.GetProducts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
        Assert.Equal(products, returnedProducts);
        Assert.Equal(5, returnedProducts.Count());
        A.CallTo(() => _fakeUow.Repository<Product>().GetAllAsync()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetProducts_WithNoProductsInDb_ReturnsOkResult_WithEmptyList()
    {
        // Arrange
        A.CallTo(() => _fakeRepository.GetAllAsync()).Returns(Task.FromResult((IEnumerable<Product>)new List<Product>()));
        A.CallTo(() => _fakeUow.Repository<Product>()).Returns(_fakeRepository);

        // Act
        var result = await _fakeController.GetProducts();

        // Assert
        var okResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        var returnedProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
        Assert.Empty(returnedProducts);
        A.CallTo(() => _fakeUow.Repository<Product>().GetAllAsync()).MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task GetProductById_WithValidId_ReturnsOkResultWithAProduct()
    {
        // Arrange
        var product = A.Dummy<Product>();
        A.CallTo(() => _fakeRepository.GetByIdAsync(product.Id)).Returns(Task.FromResult(product));
        // A.CallTo(() => _fakeUow.Repository<Product>().GetByIdAsync(product.Id)).Returns(Task.FromResult(product));
        A.CallTo(() => _fakeUow.Repository<Product>()).Returns(_fakeRepository);
        // await _fakeController.AddProduct(product);

        // Act
        var result = await _fakeController.GetProductById(product.Id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProduct = Assert.IsAssignableFrom<Product>(okResult.Value);
        Assert.Equal(product, returnedProduct);
        A.CallTo(() => _fakeUow.Repository<Product>().GetByIdAsync(product.Id)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetProductById_WithInvalidId_ReturnsNotFoundResult()
    {
        // Arrange
        var product = new Product
        {
            Name = "Test Product",
            Price = 10,
            Amount = 5
        };
        await _controller.AddProduct(product);

        // Act
        var result = await _controller.GetProductById(999);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateProduct_ReturnsOkResult()
    {
        // Arrange
        //var product = A.Dummy<Product>();
        var product = new Product
        {
            Name = "Test Product",
            Price = 10,
            Amount = 5
        };
        A.CallTo(() => _fakeRepository.GetByIdAsync(product.Id)).Returns(Task.FromResult(product));
        A.CallTo(() => _fakeUow.Repository<Product>()).Returns(_fakeRepository);

        // Act
        var result = await _fakeController.UpdateProduct(product.Id, product);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        A.CallTo(() => _fakeRepository.UpdateAsync(product)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _fakeUow.CommitAsync()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdateProduct_WithInvalidInput_ReturnsBadRequest()
    {
        // Arrange
        var product = new Product
        {
            Id = 123,
            Name = "UpdateTest Product",
            Price = 10,
            Amount = 5
        };

        await _controller.AddProduct(product);

        product.Name = "U";
        product.Price = -10;
        product.Amount = 10;
        
        // Act
        var result = await _controller.UpdateProduct(product.Id, product);

        // Assert
        Assert.IsAssignableFrom<BadRequestResult>(result);
    }

    [Fact]
    public async Task UpdateProductAmount_WithValidAmount_ReturnsOkResult()
    {
        // Arrange
        var product = new Product
        {
            Id = 123,
            Name = "UpdateTest Product",
            Price = 10,
            Amount = 5
        };

        await _controller.AddProduct(product);
        product.Amount = 10;

        // Act
        var result = await _controller.UpdateProduct(product.Id, product);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task UpdateProductAmount_WithInvalidAmount_ReturnsOkResult()
    {
        // Arrange
        var product = new Product
        {
            Id = 123,
            Name = "UpdateTest Product",
            Price = 10,
            Amount = 5
        };

        await _controller.AddProduct(product);
        product.Amount = -5;

        // Act
        var result = await _controller.UpdateProduct(product.Id, product);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task DeleteProduct_ReturnsOkResult()
    {
        // Arrange
        var product = A.Dummy<Product>();
        A.CallTo(() => _fakeRepository.GetByIdAsync(product.Id)).Returns(Task.FromResult(product));
        A.CallTo(() => _fakeUow.Repository<Product>()).Returns(_fakeRepository);

        // Act
        var result = await _fakeController.DeleteProduct(product.Id);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        A.CallTo(() => _fakeRepository.DeleteAsync(product.Id)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _fakeUow.CommitAsync()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task DeleteProduct_ReturnsNotFoundResult()
    {
        // Arrange
        var product = A.Dummy<Product>();
        A.CallTo(() => _fakeRepository.GetByIdAsync(product.Id)).Returns(Task.FromResult((Product)null));
        A.CallTo(() => _fakeUow.Repository<Product>()).Returns(_fakeRepository);

        // Act
        var result = await _fakeController.DeleteProduct(product.Id);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
        A.CallTo(() => _fakeRepository.DeleteAsync(product.Id)).MustNotHaveHappened();
        A.CallTo(() => _fakeUow.CommitAsync()).MustNotHaveHappened();
    }

    [Fact]
    public async Task DeleteProduct_WithActualProduct_ReturnsOkResult()
    {
        // Arrange
        var product = new Product
        {
            Name = "Test Product",
            Price = 10,
            Amount = 5
        };
        await _controller.AddProduct(product);

        // Act
        var result = await _controller.DeleteProduct(1);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task DeleteProduct_WithFailedDbConnection_ThrowsAggregateException()
    {
        // Arrange
        var product = new Product
        {
            Id = 111,
            Name = "Delete Test Product",
            Price = 10,
            Amount = 5
        };

        await _controller.AddProduct(product);
        await _context.Database.EnsureDeletedAsync();

        // Act
        var result = await _controller.DeleteProduct(product.Id);
        var exception = result as ObjectResult;

        // Assert
        Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, exception.StatusCode);
    }

    [Fact]
    public async Task SearchProducts_WithValidName_ReturnsOkResult()
    {
        // Arrange
        var product = new Product
        {
            Name = "SearchTest Product",
            Price = 10,
            Amount = 5
        };

        await _controller.AddProduct(product);

        // Act
        var result = await _controller.SearchProducts(product.Name);

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;

        Assert.NotNull(okResult);
        Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);

        var resultProducts = okResult.Value as IEnumerable<Product>;
        Assert.Contains(product, resultProducts);
    }

    [Fact]
    public async Task SearchProducts_WithInvalidName_ReturnsNotFound_WithEmptyList()
    {
        // Arrange
        var product = new Product
        {
            Name = "SearchTest Product",
            Price = 10,
            Amount = 5
        };

        await _controller.AddProduct(product);
        var invalidName = "Invalid Name";

        // Act
        var result = await _controller.SearchProducts(invalidName);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.NotNull(notFoundResult);
        Assert.Empty(notFoundResult.Value as IEnumerable<Product>);
    }
}
