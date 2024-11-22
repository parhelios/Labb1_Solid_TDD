namespace WebShop.Shared.Notifications;

public interface ISubject<TEntity> where TEntity : class
{
    void Attach(INotificationObserver<TEntity> observer);
    void Detach(INotificationObserver<TEntity> observer);
    void Notify(TEntity entity);
}