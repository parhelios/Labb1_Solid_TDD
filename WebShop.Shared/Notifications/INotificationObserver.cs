using WebShop.Shared.Models;

namespace WebShop.Shared.Notifications;

public interface INotificationObserver<TEntity> where TEntity : class
{
    void Update(TEntity entity); 
}