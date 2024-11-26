using WebShop.Shared.Interfaces;
using WebShop.Shared.Models;

namespace WebShop.Observer
{
    public class ProductSubject : ISubject<Product>
    {
        private readonly List<INotificationObserver<Product>> _observers = [];

        public void Attach(INotificationObserver<Product> observer)
        {
            _observers.Add(observer);
        }

        public void Detach(INotificationObserver<Product> observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(Product entity)
        {
            foreach (var observer in _observers)
            {
                observer.Update(entity);
            }
        }
    }
}
