using WebShop.Shared.Interfaces;
using WebShop.Shared.Models;

namespace WebShop.Shared.Observer;

public class PushNotification : INotificationObserver<Product>
{
    public void Update(Product entity)
    {
        Console.WriteLine($"Push Notification: New product added - {entity.Name}");

    }
}