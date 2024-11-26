namespace WebShop.Shared.Interfaces;

public interface INotificationObserver<TEntity> where TEntity : class
{
    void Update(TEntity entity);
}