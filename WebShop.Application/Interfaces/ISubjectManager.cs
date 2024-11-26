namespace WebShop.Application.Interfaces;

public interface ISubjectManager
{
    ISubject<TEntity> Subject<TEntity>() where TEntity : class;
}