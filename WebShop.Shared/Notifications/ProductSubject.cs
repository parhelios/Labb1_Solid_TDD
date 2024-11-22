using WebShop.Shared.Models;

namespace WebShop.Shared.Notifications
{
    // Subject som håller reda på observatörer och notifierar dem
    public class ProductSubject : ISubject<Product>
    {
        // Lista över registrerade observatörer
        //TODO: SE ÖVER
        private readonly List<INotificationObserver<Product>> _observers = [];

        public void Attach(INotificationObserver<Product> observer)
        {
            // Lägg till en observatör
            _observers.Add(observer);
        }

        public void Detach(INotificationObserver<Product> observer)
        {
            // Ta bort en observatör
            _observers.Remove(observer);
        }

        public void Notify(Product entity)
        {
            // Notifiera alla observatörer om en ny produkt
            foreach (var observer in _observers)
            {
                observer.Update(entity);
            }
        }
    }
}
