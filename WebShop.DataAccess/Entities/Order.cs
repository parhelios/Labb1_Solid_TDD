using System.ComponentModel.DataAnnotations;

namespace WebShop.Entities;

public class Order : IEntity
{
    [Key]
    public int Id { get; }
    public Customer Customer { get; set; }
    public IEnumerable<OrderProducts>? Products { get; set; }
}