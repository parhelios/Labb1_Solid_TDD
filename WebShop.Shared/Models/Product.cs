namespace WebShop.Shared.Models
{
    // Produktmodellen representerar en produkt i webbshoppen
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
    }
}