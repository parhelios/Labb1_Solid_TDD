using System.Linq.Expressions;
using WebShop.DataAccess.Repositories;
using WebShop.Shared.Models;

namespace WebShop.DataAccess.Strategy_Pattern;

public class SpecialProductRepository(IRepository<Product> innerRepository) : IProductRepository
{
    public async Task<Product> GetByIdAsync(int id) => await innerRepository.GetByIdAsync(id);

    public async Task<IEnumerable<Product>> GetAllAsync() => await innerRepository.GetAllAsync();

    public async Task AddAsync(Product entity) => await innerRepository.AddAsync(entity);

    public async Task UpdateAsync(Product entity) => await innerRepository.UpdateAsync(entity);

    public async Task DeleteAsync(int id) => await innerRepository.DeleteAsync(id);

    public async Task<IEnumerable<Product>> FindAsync(Expression<Func<Product, bool>> predicate) => await innerRepository.FindAsync(predicate);

    public async Task UpdateProductAmount(int id, int amount)
    {
        var product = innerRepository.GetByIdAsync(id).Result;
        if (product is null) return;

        product.Amount = amount;
        await innerRepository.UpdateAsync(product);
    }
}