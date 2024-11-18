using WebShop.DataAccess.Repositories.Interfaces;
using WebShop.Shared.Entities;

namespace WebShop.DataAccess.UnitOfWork
{
    // Gränssnitt för Unit of Work
    public interface IUnitOfWork : IDisposable
    {
        Task CommitAsync();
        IProductRepository ProductRepository { get; }
        ICustomerRepository CustomerRepository { get; }
        IOrderRepository OrderRepository { get; }
        void NotifyProductAdded(Product product);
    }
}