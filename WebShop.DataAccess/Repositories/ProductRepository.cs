using WebShop.Shared.Entities;

namespace WebShop.DataAccess.Repositories;

public class ProductRepository(MyDbContext context) : Repository<Product>(context), IProductRepository
{
    private readonly MyDbContext _repoContext = context;

    public void UpdateProductAmount(int amount, Product product)
    {
        product.Amount = amount;
        _repoContext.Products.Update(product);
    }
}