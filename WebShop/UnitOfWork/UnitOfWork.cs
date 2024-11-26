using WebShop.DataAccess;
using WebShop.Shared.Interfaces;
using WebShop.Shared.Models;

namespace WebShop.UnitOfWork;

public class UnitOfWork(MyDbContext context, IRepositoryFactory factory) : IUnitOfWork
{
    private readonly Dictionary<Type, object> _repositories = new();

    public IRepository<TEntity> Repository<TEntity>() where TEntity : class
    {
        if (_repositories.TryGetValue(typeof(TEntity), out var existingRepository))
            return (IRepository<TEntity>) existingRepository;

        var repository = factory.CreateRepository<TEntity>();
        _repositories[typeof(TEntity)] = repository;
        return repository;
    }
    
    public async Task CommitAsync()
    {
        await context.SaveChangesAsync();
    }

    public async void Dispose()
    {
        await context.DisposeAsync();
    }
}