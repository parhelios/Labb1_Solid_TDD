using WebShop.Shared.Interfaces;
using WebShop.Shared.Models;

namespace WebShop.Shared.Observer;

public class SmsNotification : INotificationObserver<Product>
{
    public void Update(Product entity)
    {
        Console.WriteLine($"SMS Notification: New product added - {entity.Name}");
    }
}