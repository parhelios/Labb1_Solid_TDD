using FakeItEasy;
using WebShop.Domain.Entities;
using WebShop.Infrastructure.Interfaces;
using WebShop.Tests.TestData;

namespace WebShop.Tests;

public class UnitOfWorkTests
{
    private readonly IUnitOfWork _fakeUow = A.Fake<IUnitOfWork>();
    private readonly IRepository<Product> _fakeProductRepository = A.Fake<IRepository<Product>>();
    private readonly IRepository<Customer> _fakeCustomerRepository = A.Fake<IRepository<Customer>>();
    private readonly IRepository<Order> _fakeOrderRepository = A.Fake<IRepository<Order>>();

    [Fact]
    public async Task CallsUnitOfWork_ReturnsRepository_OfTypeProduct()
    {
        // Arrange
        var product = A.Dummy<Product>();
        A.CallTo(()=> _fakeUow.Repository<Product>()).Returns(_fakeProductRepository);

        // Act
        await _fakeUow.Repository<Product>().AddAsync(product);

        // Assert
        A.CallTo(() => _fakeUow.Repository<Product>().AddAsync(product)).MustHaveHappenedOnceExactly();
    }


    [Fact]
    public async Task CallsUnitOfWork_ReturnsRepository_OfTypeCustomer()
    {
        // Arrange
        var customer = A.Dummy<Customer>();
        A.CallTo(() => _fakeUow.Repository<Customer>()).Returns(_fakeCustomerRepository);

        // Act
        await _fakeUow.Repository<Customer>().AddAsync(customer);

        // Assert
        A.CallTo(() => _fakeUow.Repository<Customer>().AddAsync(customer)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task CallsUnitOfWork_ReturnsRepository_OfTypeOrder()
    {
        // Arrange

        var order = A.Dummy<Order>();
        A.CallTo(() => _fakeUow.Repository<Order>()).Returns(_fakeOrderRepository);

        // Act
        await _fakeUow.Repository<Order>().AddAsync(order);

        // Assert
        A.CallTo(() => _fakeUow.Repository<Order>().AddAsync(order)).MustHaveHappenedOnceExactly();
    }
  
    [Fact]
    public async Task CallsUnitOfWork_CommitChangeIsCalled()
    {
        // Arrange
        var productService = new MockProductService(_fakeUow);
        var dummyProduct = A.Dummy<Product>();

        // Act
        await productService.AddProductAsync(dummyProduct);
        
        // Assert
        A.CallTo(() => _fakeUow.CommitAsync()).MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task AddProductAsync_CallsDisposeOnUnitOfWork()
    {
        // Arrange
        var productService = new MockProductService(_fakeUow);
        var dummyProduct = A.Dummy<Product>();

        // Act
        await productService.AddProductAsync(dummyProduct);

        // Assert
        A.CallTo(() => _fakeUow.Dispose()).MustHaveHappenedOnceExactly();
    }
}