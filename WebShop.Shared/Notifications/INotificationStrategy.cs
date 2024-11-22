namespace WebShop.Shared.Notifications;

public interface INotificationStrategy<TEntity>
{
    void Notify(TEntity entity);
}