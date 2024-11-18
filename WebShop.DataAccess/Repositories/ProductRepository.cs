using WebShop.Entities;

namespace WebShop.Repositories;

public class ProductRepository(DbContext context) : IProductRepository
{
    public Task<Entities.Product> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Entities.Product>> GetAll()
    {
        return context.Products;
    }

    public void Add(Entities.Product entity)
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