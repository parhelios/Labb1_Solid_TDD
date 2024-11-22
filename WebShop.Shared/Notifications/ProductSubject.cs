using WebShop.Shared.Models;

namespace WebShop.Shared.Notifications
{
    // Subject som håller reda på observatörer och notifierar dem
    public class ProductSubject : ISubject<Product>
    {
        // Lista över registrerade observatörer
        //TODO: SE ÖVER
        private readonly List<INotificationObserver> _observers = [];

        public void Attach(INotificationObserver observer)
        {
            // Lägg till en observatör
            _observers.Add(observer);
        }

        public void Detach(INotificationObserver observer)
        {
            // Ta bort en observatör
            _observers.Remove(observer);
        }

        public void Notify(Product product)
        {
            // Notifiera alla observatörer om en ny produkt
            foreach (var observer in _observers)
            {
                observer.Update(product);
            }
        }
    }
}
