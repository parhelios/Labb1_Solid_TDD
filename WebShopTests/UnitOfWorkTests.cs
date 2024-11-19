using FakeItEasy;
using WebShop.DataAccess.Repositories;
using WebShop.DataAccess.UnitOfWork;
using WebShop.Shared.Models;

namespace WebShopTests
{
    public class UnitOfWorkTests
    {
        //TODO: Skriv om tester i FakeItEasy
        
        private readonly IUnitOfWork _uow;
        private readonly IRepository<Product> _repository;

        public UnitOfWorkTests()
        {
            _uow = A.Fake<IUnitOfWork>();
            _repository = A.Fake<IRepository<Product>>();
        }
        
        
        // [Fact]
        // public void NotifyProductAdded_CallsObserverUpdate()
        // {
        //     // Arrange
        //     var dummyProduct = A.Dummy<Product>();
        //
        //     var mockObserver = A.Fake<INotificationObserver>();
        //
        //     var productSubject = new ProductSubject();
        //     productSubject.Attach(mockObserver);
        //
        //     var unitOfWork = new UnitOfWork(productSubject);
        //
        //     // Act
        //     // unitOfWork.NotifyProductAdded(product);
        //
        //     // Assert
        //     // Verifierar att Update-metoden kallades p� v�r mock-observat�r
        //     mockObserver.Verify(o => o.Update(product), Times.Once);
        // }
    }
}
