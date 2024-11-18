using Microsoft.EntityFrameworkCore;
using WebShop.DataAccess.Repositories;
using WebShop.DataAccess.Repositories.Interfaces;
using WebShop.Shared.Entities;
using WebShop.Shared.Notifications;

namespace WebShop.DataAccess.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly MyDbContext _context;
    private readonly Dictionary<Type, object> _repositories = new();
    private readonly ProductSubject _productSubject = new();

    public UnitOfWork(MyDbContext context)
    {
        _context = context;
    }

    public IRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        if (_repositories.Keys.Contains(typeof(TEntity)))
        {
            return _repositories[typeof(TEntity)] as IRepository<TEntity>;
        }

        IRepository<TEntity> repository = new Repository<TEntity>(_context);
        _repositories.Add(typeof(TEntity), repository);

        return repository;
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async void NotifyProductAdded(Product product)
    {
        _productSubject.Notify(product);

    }

    public async void Dispose()
    {
        await _context.DisposeAsync();
    }
}