using WebShop.Shared.Interfaces;
using WebShop.Shared.Models;
using WebShop.Shared.Observer;

namespace WebShop.DataAccess.Factory;

public class SubjectFactory : ISubjectFactory
{
    public ISubject<TEntity> CreateSubject<TEntity>() where TEntity : class
    {
        if (typeof(TEntity) == typeof(Product))
            return (ISubject<TEntity>) new ProductSubject();

        return new Subject<TEntity>();
    }
}