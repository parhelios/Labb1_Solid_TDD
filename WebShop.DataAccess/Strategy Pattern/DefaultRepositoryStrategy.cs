using WebShop.DataAccess.Factory;
using WebShop.DataAccess.Repositories;

namespace WebShop.DataAccess.Strategy_Pattern;

public class DefaultRepositoryStrategy<TEntity>(IRepositoryFactory factory) : IRepositoryStrategy<TEntity>
    where TEntity : class
{
    public IRepository<TEntity> CreateRepository()
    {
        return factory.CreateRepository<TEntity>();
    }
}