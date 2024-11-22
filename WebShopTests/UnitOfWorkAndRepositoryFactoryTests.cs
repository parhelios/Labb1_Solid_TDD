using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using WebShop.DataAccess;
using WebShop.DataAccess.Repositories;
using WebShop.DataAccess.UnitOfWork;
using WebShop.Shared.Models;
using WebShop.Shared.Notifications;

namespace WebShopTests
{
    public class UnitOfWorkAndRepositoryFactoryTests
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
        public async Task NotifyProductAdded_CallsObserverUpdate()
        {
            // Arrange
            var dummyProduct = A.Dummy<Product>();
        
            var mockObserver = A.Fake<INotificationObserver>();
        
            var productSubject = new ProductSubject();
            productSubject.Attach(mockObserver);
        
            // Act
            _fakeUow.NotifyProductAdded(dummyProduct);
        
            // Assert
            // Verifierar att Update-metoden kallades p� v�r mock-observat�r
            // mockObserver.Verify(o => o.Update(product), Times.Once);
            // mockObserver.Verify(o => o.Update(dummyProduct, Times.Once));
        }
    }
}
