using WebShop.Application.Interfaces;
using WebShop.Domain.Entities;

namespace WebShop.Infrastructure.Observer.Notification;

public class EmailNotificationObserver(string name, string email) : INotificationObserver<Product>
{
    public void Update(Product entity)
    {
        Console.WriteLine($" To {name}: {email}.\n Email Notification: New product added - {entity.Name}.");
    }
}