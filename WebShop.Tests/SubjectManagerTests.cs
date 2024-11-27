using FakeItEasy;
using WebShop.Application.Interfaces;
using WebShop.Domain.Entities;
using WebShop.Infrastructure.Observer;

namespace WebShop.Tests;

public class SubjectManagerTests
{
    private readonly ISubjectFactory _factory;
    private readonly ISubjectManager _manager;

    public SubjectManagerTests()
    {
        _factory = A.Fake<ISubjectFactory>();
        _manager = A.Fake<ISubjectManager>();
    }
    
    [Fact]
    public async Task CallsSubjectManager_ReturnsSubjectOfTypeProduct()
    {
        // Arrange
        var fakeProductSubject = A.Fake<ISubject<Product>>();
        A.CallTo(() => _manager.Subject<Product>()).Returns(fakeProductSubject);

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


}