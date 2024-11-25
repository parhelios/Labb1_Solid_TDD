namespace WebShop.Shared.Interfaces;

public interface INotificationStrategy<TEntity>
{
    void Notify(TEntity entity);
}