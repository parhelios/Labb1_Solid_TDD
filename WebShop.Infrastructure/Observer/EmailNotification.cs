using WebShop.Application.Interfaces;
using WebShop.Domain.Models;

namespace WebShop.Infrastructure.Observer;

public class EmailNotification : INotificationObserver<Product>
{
    public void Update(Product entity)
    {
        Console.WriteLine($"Email Notification: New product added - {entity.Name}");
    }
}