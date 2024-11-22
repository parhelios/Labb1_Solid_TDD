using WebShop.DataAccess.Repositories;
using WebShop.Shared.Models;
using WebShop.Shared.Models.Interfaces;

namespace WebShop.DataAccess.UnitOfWork
{
    // Gränssnitt för Unit of Work
    public interface IUnitOfWork : IDisposable
    {
        Task CommitAsync();
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
        //TODO: Flytta?
        void NotifyProductAdded(Product product);
        void NotifyCustomerAdded(Customer customer);
        void Notify<TEntity>(TEntity entity);
    }
}