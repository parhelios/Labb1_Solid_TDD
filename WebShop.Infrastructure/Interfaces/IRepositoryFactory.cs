namespace WebShop.Infrastructure.Interfaces;

public interface IRepositoryFactory
{
    IRepository<TEntity> CreateRepository<TEntity>() where TEntity : class;
}