using WebShop.DataAccess.Repositories.Interfaces;

namespace WebShop.DataAccess.Factory;

public interface IRepositoryFactory
{
    IRepository<TEntity> CreateRepository<TEntity>() where TEntity : class;
}