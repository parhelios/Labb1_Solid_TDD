using System.ComponentModel.DataAnnotations;

namespace WebShop.DataAccess.Entities;

public class Customer : IEntity
{
    [Key]
    public int Id { get; }
    public string Name { get; set; }
    public string Email { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}