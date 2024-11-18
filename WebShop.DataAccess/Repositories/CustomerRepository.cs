using WebShop.DataAccess.Repositories.Interfaces;
using WebShop.Shared.Models;

namespace WebShop.DataAccess.Repositories;

public class CustomerRepository(DbContext context) : ICustomerRepository
{
    public Task<Customer> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Customer>> GetAll()
    {
        throw new NotImplementedException();
    }

    public void Add(Customer entity)
    {
        throw new NotImplementedException();
    }

    public void Update(Customer entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(Customer entity)
    {
        throw new NotImplementedException();
    }
}