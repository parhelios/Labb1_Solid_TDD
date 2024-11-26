using WebShop.Application.Interfaces;

namespace WebShop.Infrastructure.Observer;

public class SubjectManager(ISubjectFactory subjectFactory) : ISubjectManager
{
    //TODO: Byt namn
    private readonly Dictionary<Type, object> _subjects = new();
    
    public ISubject<TEntity> Subject<TEntity>() where TEntity : class
    {
        if (_subjects.TryGetValue(typeof(TEntity), out var existingSubject))
            return (ISubject<TEntity>) existingSubject;

        var subject = subjectFactory.CreateSubject<TEntity>();
        _subjects[typeof(TEntity)] = subject;
        return subject;
    }
}