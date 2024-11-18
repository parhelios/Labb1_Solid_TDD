using WebShop.DataAccess.Repositories;

namespace WebShop.DataAccess.Factory;

public interface IRepositoryFactory
{
    IRepository<TEntity> CreateRepository<TEntity>() where TEntity : class;
}