using WebShop.Shared.Interfaces;

namespace WebShop.DataAccess.Factory;

public interface ISubjectFactory
{
    ISubject<TEntity> CreateSubject<TEntity>() where TEntity : IEntity;
}