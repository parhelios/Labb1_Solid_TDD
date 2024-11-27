using WebShop.Application.Interfaces;
using WebShop.Domain.Interfaces;

namespace WebShop.Application.Subjects;

public class Subject<TEntity> : ISubject<TEntity> where TEntity : class
{
    private readonly List<INotificationObserver<TEntity>> _observers = [];

    public void Attach(INotificationObserver<TEntity> observer)
    {
        _observers.Add(observer);
    }

    public void Detach(INotificationObserver<TEntity> observer)
    {
        _observers.Remove(observer);
    }

    public void Notify(TEntity entity)
    {
        foreach (var observer in _observers)
        {
            observer.Update(entity);
        }
    }
}