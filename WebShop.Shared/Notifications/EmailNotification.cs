using WebShop.Shared.Models;

namespace WebShop.Shared.Notifications;

// En konkret observatör som skickar e-postmeddelanden
public class EmailNotification : INotificationObserver<Product>, INotificationObserver<Customer>
{
    private readonly INotificationStrategy<Product> _productNotification;
    private readonly INotificationStrategy<Customer> _customerNotification;
    public EmailNotification(INotificationStrategy<Product> productNotification, INotificationStrategy<Customer> customerNotification)
    {
        _productNotification = productNotification;
        _customerNotification = customerNotification;
    }

    public void Update(Product product) => _productNotification.Notify(product);
    public void Update(Customer entity) => _customerNotification.Notify(entity);
}