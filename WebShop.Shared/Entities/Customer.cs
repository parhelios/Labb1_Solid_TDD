using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebShop.Shared.Entities;

public class Customer : IEntity
{
    [Key]
    public int Id { get; }
    public string Name { get; set; }
    public string Email { get; set; }
    [JsonIgnore]
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}