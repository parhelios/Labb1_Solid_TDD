using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebShop.DataAccess.Repositories.Interfaces;

namespace WebShop.DataAccess.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    internal readonly MyDbContext _context;
    internal readonly DbSet<TEntity> _dbSet;

    public Repository(MyDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task<TEntity> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = _dbSet.Find(id);

        if (entity is null)
            return;

        _dbSet.Remove(entity);
    }

    public Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        throw new NotImplementedException();
    }
}
