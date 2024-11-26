namespace WebShop.Shared.Interfaces;

public interface ISubjectFactory
{
    ISubject<TEntity> CreateSubject<TEntity>() where TEntity : class;
}