using WebShop.Shared.Interfaces;
using WebShop.Shared.Models;

namespace WebShop.Observer;

public class EmailNotification : INotificationObserver<Product>
{
    public void Update(Product entity)
    {
        Console.WriteLine($"Email Notification: New product added - {entity.Name}");
    }
}