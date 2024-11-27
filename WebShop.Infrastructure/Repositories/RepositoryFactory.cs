using WebShop.Infrastructure.DataAccess;
using WebShop.Infrastructure.Interfaces;

namespace WebShop.Infrastructure.Repositories;

public class RepositoryFactory(MyDbContext context) : IRepositoryFactory
{
    public IRepository<TEntity> CreateRepository<TEntity>() where TEntity : class
    {
        return new Repository<TEntity>(context);
    }
}