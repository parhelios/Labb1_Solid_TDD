using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebShop.Shared.Interfaces;

namespace WebShop.DataAccess.Repositories;

public class Repository<TEntity>(MyDbContext context) : IRepository<TEntity>
    where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public async Task<TEntity> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

    public async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbSet.ToListAsync();

    public async Task AddAsync(TEntity entity) => await _dbSet.AddAsync(entity);

    public async Task UpdateAsync(TEntity entity) => _dbSet.Update(entity);

    public async Task DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);

        if (entity is null)
            return;

        _dbSet.Remove(entity);
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate) => await _dbSet.Where(predicate).ToListAsync();
}
