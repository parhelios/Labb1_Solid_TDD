using WebShop.Shared.Models;

namespace WebShop.Shared.Notifications;

public class CustomerSubject : ISubject<Customer>
{
    private readonly List<INotificationObserver> _observers = [];
    
    public void Attach(INotificationObserver observer)
    {
        throw new NotImplementedException();
    }

    public void Detach(INotificationObserver observer)
    {
        throw new NotImplementedException();
    }

    public void Notify(Customer entity)
    {
        throw new NotImplementedException();
    }
}