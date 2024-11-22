using WebShop.Shared.Models;

namespace WebShop.Shared.Notifications;

public class CustomerSubject : ISubject<Customer>
{
    private readonly List<INotificationObserver<Customer>> _observers = [];
    
    public void Attach(INotificationObserver<Customer> observer)
    {
        throw new NotImplementedException();
    }

    public void Detach(INotificationObserver<Customer> observer)
    {
        throw new NotImplementedException();
    }

    public void Notify(Customer entity)
    {
        foreach (var observer in _observers)
        {
            observer.Update(entity);
        }
    }
}