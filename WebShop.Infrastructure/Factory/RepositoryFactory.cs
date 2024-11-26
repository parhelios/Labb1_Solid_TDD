using WebShop.Infrastructure.DataAccess;
using WebShop.Infrastructure.Interfaces;
using WebShop.Infrastructure.Repositories;

namespace WebShop.Infrastructure.Factory;

public class RepositoryFactory(MyDbContext context) : IRepositoryFactory
{
    public IRepository<TEntity> CreateRepository<TEntity>() where TEntity : class
    {
        return new Repository<TEntity>(context);
    }
}