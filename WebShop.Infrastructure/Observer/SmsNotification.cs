using WebShop.Application.Interfaces;
using WebShop.Domain.Models;

namespace WebShop.Infrastructure.Observer;

public class SmsNotification : INotificationObserver<Product>
{
    public void Update(Product entity)
    {
        Console.WriteLine($"SMS Notification: New product added - {entity.Name}");
    }
}