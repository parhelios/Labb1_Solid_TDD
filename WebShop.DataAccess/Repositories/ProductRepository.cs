//using WebShop.DataAccess.Repositories.Interfaces;
//using WebShop.Shared.Entities;

//namespace WebShop.DataAccess.Repositories;

//public class ProductRepository(MyDbContext context) : IProductRepository
//{
//    public async Task<Product?> GetById(int id) => context.Products.Find(id);

//    public async Task<IEnumerable<Product>> GetAll() => context.Products;

//    public async void Add(Product entity)
//    {
//        await context.Products.AddAsync(entity);
//    }

//    public async void Update(Product entity)
//    {
//        throw new NotImplementedException();
//    }

//    public void Delete(int id)
//    {
//        throw new NotImplementedException();
//    }

//    public async void UpdateProductAmount(int amount, Product product)
//    {
//        throw new NotImplementedException();
//    }
//}