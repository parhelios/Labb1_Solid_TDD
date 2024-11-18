using Moq;
using WebShop.DataAccess.UnitOfWork;
using WebShop.Shared.Entities;
using WebShop.Shared.Notifications;

namespace WebShopTests
{
    public class UnitOfWorkTests
    {
        //TODO: Skriv om tester i FakeItEasy

        
        [Fact]
        public void NotifyProductAdded_CallsObserverUpdate()
        {
            // Arrange
            var product = new Product
            {
                Name = null,
                Price = 0,
                Amount = 0
            };

            // Skapar en mock av INotificationObserver
            var mockObserver = new Mock<INotificationObserver>();

            // Skapar en instans av ProductSubject och l�gger till mock-observat�ren
            var productSubject = new ProductSubject();
            productSubject.Attach(mockObserver.Object);

            // Injicerar v�rt eget ProductSubject i UnitOfWork
            // var unitOfWork = new UnitOfWork(productSubject);

            // Act
            // unitOfWork.NotifyProductAdded(product);

            // Assert
            // Verifierar att Update-metoden kallades p� v�r mock-observat�r
            mockObserver.Verify(o => o.Update(product), Times.Once);
        }
    }
}
