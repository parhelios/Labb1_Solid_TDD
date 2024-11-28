using WebShop.Application.ObserverSubjects;

namespace WebShop.Tests;

public class ProductSubjectTests
{
    [Fact]
    public void TestProductSubjectSingleton_ReturnsSameInstance()
    {
        // Act
        var instance1 = ProductSubject.Instance;
        var instance2 = ProductSubject.Instance;

        // Assert: Verify that both references point to the same object
        Assert.Same(instance1, instance2); 
    }
    
    [Fact]
    public void TestSingletonIsThreadSafe()
    {
        // Arrange
        ProductSubject instance1 = null;
        ProductSubject instance2 = null;
        
        // Act: Simulate concurrent access
        var thread1 = new Thread(() => instance1 = ProductSubject.Instance);
        var thread2 = new Thread(() => instance2 = ProductSubject.Instance);

        thread1.Start();
        thread2.Start();
        thread1.Join();
        thread2.Join();

        // Assert
        Assert.Same(instance1, instance2);
    }
}