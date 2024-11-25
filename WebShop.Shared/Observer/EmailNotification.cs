using WebShop.Shared.Interfaces;
using WebShop.Shared.Models;

namespace WebShop.Shared.Observer;

public class EmailNotification : INotificationObserver<Product>
{
    public void Update(Product product)
    {
        Console.WriteLine($"Email Notification: New product added - {product.Name}");
    }
}