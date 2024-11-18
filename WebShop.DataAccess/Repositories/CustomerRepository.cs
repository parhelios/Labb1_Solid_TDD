//using Microsoft.EntityFrameworkCore;
//using WebShop.DataAccess.Repositories.Interfaces;
//using WebShop.Shared.Entities;

//namespace WebShop.DataAccess.Repositories;

//public class CustomerRepository(MyDbContext context) : ICustomerRepository
//{
//    public async Task<Customer> GetById(int id)
//    {
//        return await context.Customers.FindAsync(id);
//    }

//    public async Task<IEnumerable<Customer>> GetAll()
//    {
//        return await context.Customers.ToListAsync();
//    }

//    public async void Add(Customer entity)
//    {
//        await context.Customers.AddAsync(entity);
//    }

//    public void Update(Customer entity)
//    {
//         context.Update(entity);
//    }

//    public void Delete(int id)
//    {
//        context.Customers.Remove(context.Customers.Find(id));
//    }
//}