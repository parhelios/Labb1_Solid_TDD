using WebShop.DataAccess.Repositories;
using WebShop.DataAccess.Repositories.Interfaces;
using WebShop.Shared.Entities;
using WebShop.Shared.Notifications;

namespace WebShop.DataAccess.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly MyDbContext _context;
    private readonly ProductSubject _productSubject;
    public IProductRepository ProductRepository { get; private set; }
    public ICustomerRepository CustomerRepository { get; private set;  }
    public IOrderRepository OrderRepository { get; private set;  }
        
    public UnitOfWork(MyDbContext context, ProductSubject productSubject = null)
    {
        _context = context;
            
        ProductRepository = new ProductRepository(context);
        CustomerRepository = new CustomerRepository(context);
        OrderRepository = new OrderRepository(context);
        
        // Om inget ProductSubject injiceras, skapa ett nytt
        _productSubject = productSubject ?? new ProductSubject();
        //
        //     // Registrera standardobservatörer
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