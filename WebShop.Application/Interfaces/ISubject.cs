using WebShop.Domain.Interfaces;

namespace WebShop.Application.Interfaces;

public interface ISubject<TEntity> where TEntity : class
{
    void Attach(INotificationObserver<TEntity> observer);
    void Detach(INotificationObserver<TEntity> observer);
    void Notify(TEntity entity);
}