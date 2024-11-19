using System.Xml.Serialization;
using WebShop.DataAccess.Factory;
using WebShop.DataAccess.Repositories;
using WebShop.DataAccess.Strategy_Pattern;
using WebShop.Shared.Models;
using WebShop.Shared.Notifications;

namespace WebShop.DataAccess.UnitOfWork;

public class UnitOfWork(MyDbContext context, IRepositoryFactory factory) : IUnitOfWork
{
    //private readonly MyDbContext context;
    private readonly Dictionary<Type, object> _repositories = new();
    private readonly Dictionary<Type, object> _strategies = new();
    private readonly ProductSubject _productSubject = new();

    public void AddStrategy<TEntity>(IRepositoryStrategy<TEntity> strategy) where TEntity : class
    {
        var repository = strategy.CreateRepository();
        _strategies[typeof(TEntity)] = strategy;
    }

    public IRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        if (_repositories.TryGetValue(typeof(TEntity), out var existingRepository))
        {
            return (IRepository<TEntity>) existingRepository;
        }

        if (_strategies.TryGetValue(typeof(TEntity), out var strategy))
        {
            var repository = ((IRepositoryStrategy<TEntity>)strategy).CreateRepository();
            _repositories[typeof(TEntity)] = repository;
            return repository;
        }

        var defaultRepository = factory.CreateRepository<TEntity>();
        _repositories[typeof(TEntity)] = defaultRepository;
        return defaultRepository;
    }

    public async Task CommitAsync()
    {
        await context.SaveChangesAsync();
    }

    public void NotifyProductAdded(Product product)
    {
        _productSubject.Notify(product);
    }

    public async void Dispose()
    {
        await context.DisposeAsync();
    }
}