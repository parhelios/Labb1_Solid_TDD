using WebShop.DataAccess.Repositories;

namespace WebShop.Factory;

public interface IRepositoryFactory
{
    IRepository<TEntity> CreateRepository<TEntity>() where TEntity : class;
}