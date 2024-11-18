using WebShop.Shared.Entities;

namespace WebShop.DataAccess.Repositories.Interfaces
{
    // Gränssnitt för produktrepositoryt enligt Repository Pattern
    public interface IProductRepository : IRepository<Product>
    {
        void UpdateProductAmount(int amount, Product product); // Uppdaterar antalet produkter
    }
}
