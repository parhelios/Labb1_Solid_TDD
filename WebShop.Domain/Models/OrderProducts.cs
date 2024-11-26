using WebShop.Domain.Interfaces;

namespace WebShop.Domain.Models;

public class OrderProducts : IEntity
{
    public int Id { get; set; }
    public Order Order { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
}