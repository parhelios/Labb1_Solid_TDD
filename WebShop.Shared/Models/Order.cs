using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebShop.Shared.Models;

public class Order : IEntity
{
    [Key]
    public int Id { get; }
    public Customer Customer { get; set; }
    [JsonIgnore]
    public ICollection<OrderProducts>? Products { get; set; }
}