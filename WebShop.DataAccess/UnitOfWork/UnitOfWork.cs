using Microsoft.EntityFrameworkCore;
using WebShop.DataAccess.Factory;
using WebShop.DataAccess.Repositories;
using WebShop.DataAccess.Repositories.Interfaces;
using WebShop.Shared.Entities;
using WebShop.Shared.Notifications;

namespace WebShop.DataAccess.UnitOfWork;

public class UnitOfWork(MyDbContext context, IRepositoryFactory factory) : IUnitOfWork
{
    private readonly Dictionary<Type, object> _repositories = new();
    private readonly ProductSubject _productSubject = new();

    public IRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        if (_repositories.TryGetValue(typeof(TEntity), out var existingRepository))
        {
            return (IRepository<TEntity>) existingRepository;
        }

        var repository = factory.CreateRepository<TEntity>();
        _repositories[typeof(TEntity)] = repository;
        return repository;
    }

    public async Task CommitAsync()
    {
        await context.SaveChangesAsync();
    }

    public async void NotifyProductAdded(Product product)
    {
        _productSubject.Notify(product);

    }

    public async void Dispose()
    {
        await context.DisposeAsync();
    }
}