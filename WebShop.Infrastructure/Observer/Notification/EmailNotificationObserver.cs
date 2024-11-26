using WebShop.Application.Interfaces;
using WebShop.Domain.Entities;

namespace WebShop.Infrastructure.Observer.Notification;

public class EmailNotificationObserver : INotificationObserver<Product>
{
    public string Name { get; set; }
    public string Email { get; set; }

    public void Update(Product entity)
    {
        Console.WriteLine($" To {Name}: {Email}.\n Email Notification: New product added - {entity.Name}.");
    }
}