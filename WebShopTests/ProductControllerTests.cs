using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShop.Application.Interfaces;
using WebShop.Controllers;
using WebShop.DataAccess;
using WebShop.Domain.Models;
using WebShop.Infrastructure.DataAccess;
using WebShop.Infrastructure.Factory;
using WebShop.Infrastructure.Interfaces;
using WebShop.Infrastructure.Observer;
using WebShop.Infrastructure.UnitOfWork;

namespace WebShopTests;

public class ProductControllerTests
{
    private readonly MyDbContext _context;
    private readonly IRepositoryFactory _factory;
    private readonly SubjectFactory _subjectFactory;
    private readonly IUnitOfWork _uow;
    private readonly SubjectManager _subjectManager;
    private readonly ProductController _controller;

    private readonly IUnitOfWork _fakeUow;
    private readonly ISubjectManager _fakeSubjectManager;
    private readonly ProductController _fakeController;
    private readonly IRepository<Product> _fakeRepository;

    public ProductControllerTests()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _context = new MyDbContext(options);
        _factory = new RepositoryFactory(_context);
        _subjectFactory = new SubjectFactory();

        _uow = new UnitOfWork(_context, _factory);
        _subjectManager = new SubjectManager(_subjectFactory);
        _controller = new ProductController(_uow, _subjectManager);

        _fakeUow = A.Fake<IUnitOfWork>();
        _fakeSubjectManager = A.Fake<ISubjectManager>();
        _fakeController = new ProductController(_fakeUow, _fakeSubjectManager);
        _fakeRepository = A.Fake<IRepository<Product>>();
    }

    [Fact]
    public async Task AddProduct_WithValidData_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var product = new Product
        {
            Name = "Test Product",
            Price = 10,
            Amount = 5
        };

        // Act
        var result = await _fakeController.AddProduct(product);

        // Assert
        Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(product, ((CreatedAtActionResult)result).Value);

        A.CallTo(() => _fakeUow.CommitAsync()).MustHaveHappenedOnceExactly();
        // A.CallTo(() => _fakeUow.Subject<Product>().Notify(A<Product>._)).MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task AddProduct_WithValidData_ReturnsCreatedAtActionResult_InMemoryDb()
    {
        // Arrange
        var product = new Product
        {
            Name = "Test Product",
            Price = 10,
            Amount = 5
        };
        
        // Act
        var result = await _controller.AddProduct(product);

        // Assert
        Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(product, ((CreatedAtActionResult)result).Value);
        
        await _context.Database.EnsureDeletedAsync();
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
        
        // Act
        var result = await _controller.AddProduct(product);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, ((BadRequestObjectResult)result).StatusCode);
        
        await _context.Database.EnsureDeletedAsync();
    }

    [Fact]
    public async Task AddProduct_WithInvalidName_ReturnsBadRequestResult()
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
        Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        Assert.Equal(400, ((BadRequestObjectResult)result).StatusCode);
        Assert.Equal("Invalid product data.", ((BadRequestObjectResult)result).Value);
    }
    
    [Fact]
    public async Task AddProduct_WithInvalidPrice_ReturnsBadRequestResult()
    {
        // Arrange
        var product = new Product
        {
            Name = "T",
            Amount = 20,
            Price = -5
        };

        // Act
        var result = await _controller.AddProduct(product);

        // Assert
        Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        Assert.Equal(400, ((BadRequestObjectResult)result).StatusCode);
        Assert.Equal("Invalid product data.", ((BadRequestObjectResult)result).Value);
    }
    
    [Fact]
    public async Task AddProduct_WhenRepositoryThrowsException_ReturnsInternalServerError()
    {
        //Arrange
        var product = new Product
        {
            Name = "Test Product",
            Price = 10,
            Amount = 5
        };
        A.CallTo(() => _fakeRepository.AddAsync(product)).Throws(new Exception());
        A.CallTo(() => _fakeUow.Repository<Product>()).Returns(_fakeRepository);

        //Act
        var result = await _fakeController.AddProduct(product);

        //Assert
        var objectResult = Assert.IsType<ObjectResult>(result);

        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Internal server error.", objectResult.Value);

        A.CallTo(() => _fakeUow.Dispose()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetProducts_ReturnsOkResult_WithAListOfProducts()
    {
        // Arrange
        var products = A.CollectionOfDummy<Product>(5); 

        A.CallTo(() => _fakeRepository.GetAllAsync()).Returns(products);
        A.CallTo(() => _fakeUow.Repository<Product>()).Returns(_fakeRepository);

        // Act
        var result = await _fakeController.GetProducts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);  
        var returnProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);  
        Assert.Equal(products.Count, returnProducts.Count());

        A.CallTo(() => _fakeRepository.GetAllAsync()).MustHaveHappenedOnceExactly();
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
        A.CallTo(() => _fakeRepository.GetAllAsync()).MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task GetProductById_WithValidId_ReturnsOkResultWithAProduct()
    {
        // Arrange
        var product = A.Dummy<Product>();
        A.CallTo(() => _fakeRepository.GetByIdAsync(product.Id)).Returns(Task.FromResult(product));
        A.CallTo(() => _fakeUow.Repository<Product>()).Returns(_fakeRepository);

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
        
        await _context.Database.EnsureDeletedAsync();
    }

    [Fact]
    public async Task UpdateProduct_WithValidData_ReturnsOkResultWithSuccessMessage()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Price = 10,
            Amount = 5
        };
        await _controller.AddProduct(product);
        
        var newProduct = new Product
        {
            Id = 1,
            Name = "Updated Test Product",
            Price = 20,
            Amount = 10
        };
        // Act
        var result = await _controller.UpdateProduct(product.Id, newProduct);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal($"Product {product.Id} updated successfully.", okResult.Value);
    }
    
    [Fact]
    public async Task UpdateProduct_WithValidData_ReturnsOkResult_Mock()
    {
        // Arrange
        var product = new Product
        {
            Name = "Test Product",
            Price = 10,
            Amount = 5
        };
        A.CallTo(() => _fakeRepository.GetByIdAsync(product.Id)).Returns(Task.FromResult(product));
        A.CallTo(() => _fakeUow.Repository<Product>()).Returns(_fakeRepository);
        product.Name = "New Product";

        // Act
        var result = await _fakeController.UpdateProduct(product.Id, product);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        A.CallTo(() => _fakeRepository.UpdateAsync(product)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _fakeUow.CommitAsync()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdateProduct_WithInvalidData_ReturnsBadRequest()
    {
        await _context.Database.EnsureDeletedAsync();
        
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
        Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        
        await _context.Database.EnsureDeletedAsync();
    }
    
    [Fact]
    public async Task UpdateProduct_WithNoObjectInDb_ReturnsNotFoundResult()
    {
        //Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Price = 10,
            Amount = 5
        };

        //Act
        var result = await _controller.UpdateProduct(product.Id, product);

        //Assert
        var objectResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, objectResult.StatusCode);
        Assert.Equal($"No product with id {product.Id} was found.", objectResult.Value);
        
        await _context.Database.EnsureDeletedAsync();
    }
    
    // [Fact]
    public async Task UpdateProduct_ThrowException_ActivateDisposeAndSendStatusCode500()
    {
        //Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Price = 10,
            Amount = 5
        };
        A.CallTo(() => _fakeRepository.GetByIdAsync(1)).Returns(Task.FromResult(product));
        A.CallTo(() => _fakeRepository.UpdateAsync(A<Product>._)).Throws(new Exception("Test"));
        A.CallTo(() => _fakeUow.Repository<Product>()).Returns(_fakeRepository);

        await _controller.AddProduct(product);
        product.Name = "New Test Product";
        
        //Act
        var result = await _controller.UpdateProduct(1, product);

        //Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("Internal server error", objectResult.Value);
        A.CallTo(() => _fakeUow.Dispose()).MustHaveHappened();
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
        var updatedProduct = await _uow.Repository<Product>().GetByIdAsync(product.Id);
        Assert.Equal(product, updatedProduct);
        
        await _context.Database.EnsureDeletedAsync();
    }

    [Fact]
    public async Task UpdateProductAmount_WithInvalidAmount_ReturnsBadRequest()
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
        
        _context.Database.EnsureDeleted();
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
