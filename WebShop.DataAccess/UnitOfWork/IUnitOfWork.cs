using WebShop.DataAccess.Repositories;
using WebShop.Shared.Interfaces;
using WebShop.Shared.Models;

namespace WebShop.DataAccess.UnitOfWork
{
    // Gränssnitt för Unit of Work
    public interface IUnitOfWork : IDisposable
    {
        Task CommitAsync();
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;

        ISubject<TEntity> Subject<TEntity>() where TEntity : class;
        //TODO: Flytta?
        void NotifyProductAdded(Product product);
        void NotifyAdded(IEntity entity);
    }
}