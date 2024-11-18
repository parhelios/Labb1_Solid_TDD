using WebShop.DataAccess.Repositories.Interfaces;
using WebShop.Shared.Entities;

namespace WebShop.DataAccess.Repositories;

public class ProductRepository(MyDbContext context) : Repository<Product>(context), IProductRepository
{
    public void UpdateProductAmount(int amount, Product product)
    {
        product.Amount = amount;
        context.Products.Update(product);
    }
}