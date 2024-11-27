namespace WebShop.Infrastructure.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task CommitAsync();
    IRepository<TEntity> Repository<TEntity>() where TEntity : class;
}