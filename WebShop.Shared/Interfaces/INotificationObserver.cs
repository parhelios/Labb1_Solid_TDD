using WebShop.Shared.Models;

namespace WebShop.Shared.Interfaces;

public interface INotificationObserver<TEntity> where TEntity : IEntity
{
    void Update(TEntity entity);
}