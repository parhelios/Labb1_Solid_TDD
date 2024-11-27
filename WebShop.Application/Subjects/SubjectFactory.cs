using WebShop.Application.Interfaces;
using WebShop.Domain.Entities;

namespace WebShop.Application.Subjects;

public class SubjectFactory : ISubjectFactory
{
    public ISubject<TEntity> CreateSubject<TEntity>() where TEntity : class
    {
        if (typeof(TEntity) == typeof(Product))
            return (ISubject<TEntity>) new ProductSubject();

        return new Subject<TEntity>();
    }
}