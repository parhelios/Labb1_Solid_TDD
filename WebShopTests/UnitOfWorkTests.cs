using FakeItEasy;
using WebShop.Observer;
using WebShop.Shared.Interfaces;
using WebShop.Shared.Models;

namespace WebShopTests
{
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
        public Task NotifyProductAdded_CallsObserverUpdate()
        {
            // Arrange
            var dummyProduct = A.Dummy<Product>();
            var mockObserver = A.Fake<INotificationObserver<Product>>();
            var productSubject = new ProductSubject();
            productSubject.Attach(mockObserver);
        
            // Act
            _fakeUow.Subject<Product>().Notify(dummyProduct);
        
            // Assert
            A.CallTo(()=> _fakeUow.Subject<Product>()).MustHaveHappenedOnceExactly();
            // A.CallTo(()=> _fakeUow.Subject<Product>().Notify(dummyProduct)).MustHaveHappenedOnceExactly();

            return Task.CompletedTask;
        }
    }
}
