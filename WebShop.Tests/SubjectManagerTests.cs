using FakeItEasy;
using WebShop.Application.Interfaces;
using WebShop.Application.Subjects;
using WebShop.Domain.Entities;
using WebShop.Domain.Interfaces;

namespace WebShop.Tests;

public class SubjectManagerTests
{
    private readonly ISubjectManager _manager = A.Fake<ISubjectManager>();
    private readonly ISubject<Product> _subject = A.Fake<ISubject<Product>>();

    [Fact]
    public async Task CallsSubjectManager_ReturnsSubjectOfTypeProduct()
    {
        // Arrange
        A.CallTo(() => _manager.Subject<Product>()).Returns(_subject);

        // Act
        var result = _manager.Subject<Product>();

        // Assert
        Assert.IsAssignableFrom<ISubject<Product>>(result); 
        A.CallTo(() => _manager.Subject<Product>()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task NotifyProductAdded_CallsObserverUpdate()
    {
        // Arrange
        var dummyProduct = A.Dummy<Product>();
        var mockObserver = A.Fake<INotificationObserver<Product>>();

        // Real ProductSubject to simulate behavior
        var productSubject = new ProductSubject();
        productSubject.Attach(mockObserver);

        A.CallTo(() => _manager.Subject<Product>()).Returns(productSubject);

        // Act
        _manager.Subject<Product>().Notify(dummyProduct);

        // Assert
        A.CallTo(() => mockObserver.Update(dummyProduct)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _manager.Subject<Product>()).MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task AttachObserverToSubject_MustHaveHappenedOnce()
    {
        // Arrange
        var dummyProduct = A.Dummy<Product>();
        var mockObserver = A.Fake<INotificationObserver<Product>>();
        
        // Act
        _subject.Attach(mockObserver);
        
        // Assert
        A.CallTo(()=> _subject.Attach(mockObserver)).MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task DetachObserverFromSubject_MustHaveHappenedOnce()
    {
        // Arrange
        var mockObserver = A.Fake<INotificationObserver<Product>>();

        _subject.Attach(mockObserver);
        
        // Act
        _subject.Detach(mockObserver);
        
        // Assert
        A.CallTo(()=> _subject.Detach(mockObserver)).MustHaveHappenedOnceExactly();
    }


}