using WebShop.Infrastructure.DataAccess;
using WebShop.Infrastructure.Interfaces;

namespace WebShop.Infrastructure.Repositories;

public class RepositoryFactory(MyDbContext context) : IRepositoryFactory
{
    public IRepository<TEntity> CreateRepository<TEntity>() where TEntity : class
    {
        //Possible to add more logic here to determine which repository to return, either generic och specific.
        
        return new Repository<TEntity>(context);
    }
}