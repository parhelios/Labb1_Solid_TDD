using WebShop.Application.Interfaces;
using WebShop.Domain.Entities;
using WebShop.Infrastructure.Observer.Notification;

namespace WebShop.Infrastructure.Observer.ObserverPatternData;

public class PopulateObserverData
{
    private static ISubjectManager _manager;

    public PopulateObserverData(ISubjectManager manager)
    {
        _manager = manager;
    }
    
    public void PopulateAllObservers()
    {
        PopulateEmailNotificationObserver();
        PopulatePushNotificationObserver();
        PopulateSmsNotificationObserver();
    }

    private void PopulateEmailNotificationObserver()
    {
        var emailObserver = new EmailNotificationObserver();
        emailObserver.Name = "John Doe";
        emailObserver.Email = "a@a.a";

        var subject = _manager.Subject<Product>();
        subject.Attach(emailObserver);
    }

    private void PopulatePushNotificationObserver()
    {
        var pushObserver = new PushNotificationObserver();

        var subject = _manager.Subject<Product>();
        subject.Attach(pushObserver);
    }

    private void PopulateSmsNotificationObserver()
    {
        var smsObserver = new SmsNotificationObserver();

        var subject = _manager.Subject<Product>();
        subject.Attach(smsObserver);
    }
}