using WebShop.Shared.Entities;

namespace WebShop.DataAccess.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        void UpdateProductAmount(int id, int amount);
    }
}
