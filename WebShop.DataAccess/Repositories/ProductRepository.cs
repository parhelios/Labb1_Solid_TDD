using WebShop.Shared.Models;

namespace WebShop.DataAccess.Repositories;

public class ProductRepository(MyDbContext context) : Repository<Product>(context), IProductRepository
{
    public void UpdateProductAmount(int id, int amount)
    {
        var product = context.Products.Find(id);
        if (product is null) return;

        product.Amount = amount;
        context.Products.Update(product);
    }
}