namespace WebShop.Repositories
{
    // Gränssnitt för produktrepositoryt enligt Repository Pattern
    public interface IProductRepository
    {
        //TODO: Add CRUD
        IEnumerable<Product> GetAll(); // Hämtar alla produkter
        void Add(Product product); // Lägger till en ny produkt
    }
}
