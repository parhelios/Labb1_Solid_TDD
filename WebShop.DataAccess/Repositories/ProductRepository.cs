using WebShop.DataAccess.Repositories.Interfaces;
using WebShop.Shared.Models;

namespace WebShop.DataAccess.Repositories;

public class ProductRepository(DbContext context) : IProductRepository
{
    public async Task<Entities.Product?> GetById(int id) => context.Products.Find(id);

    public async Task<IEnumerable<Entities.Product>> GetAll() => context.Products;

    public async void Add(Entities.Product entity)
    {
        throw new NotImplementedException();
    }

    public void Update(Entities.Product entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(Entities.Product entity)
    {
        throw new NotImplementedException();
    }

    public void UpdateProductAmount(int amount, Product product)
    {
        throw new NotImplementedException();
    }
}