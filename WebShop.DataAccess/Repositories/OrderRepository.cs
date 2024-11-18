using WebShop.DataAccess.Repositories.Interfaces;
using WebShop.Shared.Entities;

namespace WebShop.DataAccess.Repositories;

public class OrderRepository(MyDbContext context) : IOrderRepository
{
    public Task<Order> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Order>> GetAll()
    {
        throw new NotImplementedException();
    }

    public void Add(Order entity)
    {
        throw new NotImplementedException();
    }

    public void Update(Order entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(Order entity)
    {
        throw new NotImplementedException();
    }
}