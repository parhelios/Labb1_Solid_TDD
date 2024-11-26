using WebShop.Shared.Models;

namespace WebShop.Shared.Interfaces
{
    // Gränssnitt för Unit of Work
    public interface IUnitOfWork : IDisposable
    {
        Task CommitAsync();
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
    }
}