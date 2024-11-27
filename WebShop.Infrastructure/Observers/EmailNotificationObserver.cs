using WebShop.Domain.Entities;
using WebShop.Domain.Interfaces;

namespace WebShop.Infrastructure.Observers;

public class EmailNotificationObserver : INotificationObserver<Product>
{
    public string Name { get; set; }
    public string Email { get; set; }

    public void Update(Product entity)
    {
        Console.WriteLine($" To {Name}: {Email}.\nEmail Notification: New product added - {entity.Name}.");
    }
}