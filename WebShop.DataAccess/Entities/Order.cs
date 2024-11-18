using System.ComponentModel.DataAnnotations;

namespace WebShop.DataAccess.Entities;

public class Order : IEntity
{
    [Key]
    public int Id { get; }
    public Customer Customer { get; set; }
    public ICollection<OrderProducts>? Products { get; set; }
}