namespace WebShop.Notifications
{
    // En konkret observatör som skickar e-postmeddelanden
    public class EmailNotification : INotificationObserver
    {
        public void Update(Product product)
        {
            //TODO: Epostlogik
            // Ska faktisk email-funktionalitet implementeras? 
            
            // Här skulle du implementera logik för att skicka ett e-postmeddelande
            // För enkelhetens skull skriver vi ut till konsolen
            Console.WriteLine($"Email Notification: New product added - {product.Name}");
        }
    }
}
