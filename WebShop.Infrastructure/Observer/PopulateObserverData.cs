using WebShop.Application.Interfaces;
using WebShop.Domain.Entities;
using WebShop.Infrastructure.Observer.Notification;

namespace WebShop.Infrastructure.Observer;

public class PopulateObserverData
{
    private static ISubjectManager _manager;

    public PopulateObserverData(ISubjectManager manager)
    {
        _manager = manager;
    }

    public void Populate()
    {
        var emailObserver = new EmailNotificationObserver();
        emailObserver.Name = "John Doe";
        emailObserver.Email = "a@a.a";

        var subject = _manager.Subject<Product>();
        subject.Attach(emailObserver);
    }
}