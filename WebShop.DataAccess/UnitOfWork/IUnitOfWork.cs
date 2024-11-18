using WebShop.DataAccess.Repositories.Interfaces;
using WebShop.Shared.Entities;

namespace WebShop.DataAccess.UnitOfWork
{
    // Gränssnitt för Unit of Work
    public interface IUnitOfWork : IDisposable
    {
        Task CommitAsync();
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
        void NotifyProductAdded(Product product);
    }
}