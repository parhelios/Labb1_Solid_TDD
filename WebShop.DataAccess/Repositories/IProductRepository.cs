using WebShop.Shared.Models;

namespace WebShop.DataAccess.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task UpdateProductAmount(int id, int amount);
    }
}
