using WebShop.Shared.Models;

namespace WebShop.DataAccess.Repositories.Interfaces
{
    // Gränssnitt för produktrepositoryt enligt Repository Pattern
    public interface IProductRepository : IRepository<Entities.Product>
    {
        void UpdateProductAmount(int amount, Product product); // Uppdaterar antalet produkter
    }
}
