using WebShop.DataAccess.Repositories;

namespace WebShop.DataAccess.Strategy_Pattern;

public interface IRepositoryStrategy<TEntity> where TEntity : class
{
    IRepository<TEntity> CreateRepository();
}