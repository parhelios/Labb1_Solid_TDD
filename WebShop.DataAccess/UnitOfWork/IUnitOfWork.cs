using WebShop.DataAccess.Repositories;
using WebShop.DataAccess.Strategy_Pattern;
using WebShop.Shared.Models;

namespace WebShop.DataAccess.UnitOfWork
{
    // Gränssnitt för Unit of Work
    public interface IUnitOfWork : IDisposable
    {
        Task CommitAsync();
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
        void AddStrategy<TEntity>(IRepositoryStrategy<TEntity> strategy) where TEntity : class;
        void NotifyProductAdded(Product product);
    }
}