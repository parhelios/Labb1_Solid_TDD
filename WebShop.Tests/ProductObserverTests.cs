using FakeItEasy;
using WebShop.Application.Interfaces;
using WebShop.Application.Subjects;
using WebShop.Domain.Entities;
using WebShop.Domain.Interfaces;

namespace WebShop.Tests;

public class ProductObserverTests
{
    private readonly ISubject<Product> _subject = new ProductSubject();
    private readonly INotificationObserver<Product> _observer = A.Fake<INotificationObserver<Product>>();
    
    [Fact]
    public void AttachObserver_ObserverUpdateGetsCalled()
    {
        // Arrange
        var product = A.Dummy<Product>();
        _subject.Attach(_observer);

        // Act
        _subject.Notify(product);

        // Assert
        A.CallTo(() => _observer.Update(product)).MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public void AttachMultipleObservers_ReceiveNotification()
    {
        // Arrange
        var secondObserver = A.Fake<INotificationObserver<Product>>();
        var product = A.Dummy<Product>();
        _subject.Attach(_observer);
        _subject.Attach(secondObserver);

        // Act
        _subject.Notify(product);

        // Assert
        A.CallTo(() => _observer.Update(product)).MustHaveHappenedOnceExactly();
        A.CallTo(() => secondObserver.Update(product)).MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public void NotifyWithNoObservers_DoesNotThrow()
    {
        // Arrange
        var product = A.Dummy<Product>();

        // Act & Assert
        var exception = Record.Exception(() => _subject.Notify(product));
        Assert.Null(exception);
    }
}