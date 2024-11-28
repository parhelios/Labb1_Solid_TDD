using WebShop.Application.Interfaces;
using WebShop.Domain.Entities;
using WebShop.Domain.Interfaces;

namespace WebShop.Application.ObserverSubjects;

public class ProductSubject : ISubject<Product>
{
    private readonly List<INotificationObserver<Product>> _observers = [];
    private static ProductSubject _instance;

    public static ProductSubject Instance
    {
        get
        {
            if (_instance == null)
                _instance = new ProductSubject();
            return _instance;
        }
    } 

    public void Attach(INotificationObserver<Product> observer)
    {
        _observers.Add(observer);
    }

    public void Detach(INotificationObserver<Product> observer)
    {
        _observers.Remove(observer);
    }

    public void Notify(Product entity)
    {
        foreach (var observer in _observers)
            observer.Update(entity);
    }
}