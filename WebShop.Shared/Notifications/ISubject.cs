namespace WebShop.Shared.Notifications;

public interface ISubject<TEntity> where TEntity : class
{
    void Attach(INotificationObserver observer);
    void Detach(INotificationObserver observer);
    void Notify(TEntity entity);
}