using WebShop.DataAccess.Repositories;
using WebShop.DataAccess.Repositories.Interfaces;
using WebShop.Shared.Models;
using WebShop.Shared.Notifications;

namespace WebShop.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        private readonly ProductSubject _productSubject;
        public IProductRepository Products { get; }
        public ICustomerRepository Customers { get; }
        public IOrderRepository Orders { get; }
        
        public UnitOfWork(DbContext context)
        {
            _context = context;
            
            Products = new ProductRepository(context);
            Customers = new CustomerRepository(context);
            Orders = new OrderRepository(context);
        }
        
        // Konstruktor används för tillfället av Observer pattern
        public UnitOfWork(ProductSubject productSubject = null)
        {
            Products = null;

            // Om inget ProductSubject injiceras, skapa ett nytt
            _productSubject = productSubject ?? new ProductSubject();

            // Registrera standardobservatörer
            _productSubject.Attach(new EmailNotification());
        }

        public void NotifyProductAdded(Product product)
        {
            _productSubject.Notify(product);
        }
        
        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async void Dispose()
        {
            await _context.DisposeAsync();
        }
    }
}
