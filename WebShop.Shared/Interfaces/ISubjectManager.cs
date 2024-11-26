namespace WebShop.Shared.Interfaces;

public interface ISubjectManager
{
    ISubject<TEntity> Subject<TEntity>() where TEntity : class;
}