using WebShop.DataAccess.Repositories;
using WebShop.Shared.Models;

namespace WebShop.DataAccess.UnitOfWork
{
    // Gränssnitt för Unit of Work
    public interface IUnitOfWork : IDisposable
    {
        Task CommitAsync();
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
        //TODO: Flytta?
        void NotifyProductAdded(Product product);
    }
}