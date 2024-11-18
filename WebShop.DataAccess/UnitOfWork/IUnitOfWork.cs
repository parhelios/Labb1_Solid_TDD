using WebShop.DataAccess.Repositories.Interfaces;
using WebShop.Shared.Models;

namespace WebShop.DataAccess.UnitOfWork
{
    // Gränssnitt för Unit of Work
    public interface IUnitOfWork : IDisposable
    {
        //TODO: Se över 
        Task CommitAsync();
        IProductRepository Products { get; }
        ICustomerRepository Customers { get; }
        IOrderRepository Orders { get; }
        void NotifyProductAdded(Product product);
    }
}