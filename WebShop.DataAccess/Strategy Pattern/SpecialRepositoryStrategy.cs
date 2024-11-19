using WebShop.DataAccess.Factory;
using WebShop.DataAccess.Repositories;

namespace WebShop.DataAccess.Strategy_Pattern;

public class SpecialRepositoryStrategy<TEntity>(IRepositoryFactory factory) : IRepositoryStrategy<TEntity> 
    where TEntity : class
{
    public IRepository<TEntity> CreateRepository()
    {
        var repository = factory.CreateRepository<TEntity>();

        return repository switch
        {
            ProductRepository productRepository =>
                (IRepository<TEntity>) new SpecialProductRepository(productRepository),
            _ => repository
        };
    }
}