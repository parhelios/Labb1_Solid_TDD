using WebShop.Shared.Interfaces;

namespace WebShop.Factory;

public interface ISubjectFactory
{
    ISubject<TEntity> CreateSubject<TEntity>() where TEntity : class;
}