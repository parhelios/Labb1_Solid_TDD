using WebShop.DataAccess;
using WebShop.DataAccess.Repositories;
using WebShop.Shared.Interfaces;

namespace WebShop.Factory;

public class RepositoryFactory(MyDbContext context) : IRepositoryFactory
{
    public IRepository<TEntity> CreateRepository<TEntity>() where TEntity : class
    {
        return new Repository<TEntity>(context);
    }
}