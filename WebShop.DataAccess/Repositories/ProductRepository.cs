using WebShop.Entities;

namespace WebShop.Repositories;

public class ProductRepository(DbContext context) : IProductRepository
{
    public Entities.Product GetById(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Entities.Product> GetAll()
    {
        throw new NotImplementedException();
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