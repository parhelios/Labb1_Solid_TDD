using WebShop.Domain.Entities;
using WebShop.Infrastructure.Interfaces;

namespace WebShop.Tests.TestData;

public class MockProductService(IUnitOfWork unitOfWork)
{
    public async Task AddProductAsync(Product product)
    {
        await unitOfWork.Repository<Product>().AddAsync(product);
        await unitOfWork.CommitAsync();
        unitOfWork.Dispose();
    }
}