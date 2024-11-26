using WebShop.DataAccess;
using WebShop.DataAccess.Factory;
using WebShop.DataAccess.Repositories;
using WebShop.Shared.Interfaces;
using WebShop.Shared.Models;

namespace WebShop.UnitOfWork;

public class UnitOfWork(MyDbContext context, IRepositoryFactory factory, ISubjectFactory subjectFactory) : IUnitOfWork
{
    private readonly Dictionary<Type, object> _repositories = new();
    private readonly Dictionary<Type, object> _subjects = new();
    private ISubject<IEntity> _subject; //TODO: Ta bort?

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
    
    public ISubject<TEntity> Subject<TEntity>() where TEntity : class
    {
        if (_subjects.TryGetValue(typeof(TEntity), out var existingSubject))
        {
            return (ISubject<TEntity>) existingSubject;
        }

        var subject = subjectFactory.CreateSubject<TEntity>();
        _subjects[typeof(TEntity)] = subject;
        return subject;
    }

    public void NotifyProductAdded(Product product)
    {
        //TODO: Flytta?
        _subject.Notify(product);
    }

    public void NotifyAdded(IEntity entity)
    {
        //TODO: Ta bort
        _subject = Subject<IEntity>();
        _subject.Notify(entity);
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