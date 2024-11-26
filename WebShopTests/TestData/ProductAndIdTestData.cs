using System.Collections;
using WebShop.Domain.Entities;

namespace WebShopTests.TestData;

public class ProductAndIdTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return [
            new Product { Id = 11, Name = "Product Add Test", Price = 11 },
            new[] { 77, 54, 109, 1099 }
        ];
        yield return
        [
            new[]
            {
                new Product { Id = 1, Name = "Product Add Test 1", Price = 10, Amount = 5 },
                new Product { Id = 2, Name = "Product Add Test 2", Price = 10, Amount = 6 },
                new Product { Id = 3, Name = "Product Add Test 3", Price = 10, Amount = 7 },
                new Product { Id = 4, Name = "Product Add Test 4", Price = 10, Amount = 8 },
            },
            new[] { 77, 54, 109, 1099 }
        ];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}