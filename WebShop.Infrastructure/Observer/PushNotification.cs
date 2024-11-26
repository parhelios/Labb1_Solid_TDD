using WebShop.Application.Interfaces;
using WebShop.Domain.Entities;

namespace WebShop.Infrastructure.Observer;

public class PushNotification : INotificationObserver<Product>
{
    public void Update(Product entity)
    {
        Console.WriteLine($"Push Notification: New product added - {entity.Name}");

    }
}