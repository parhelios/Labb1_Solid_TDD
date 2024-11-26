using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebShop.Domain.Interfaces;

namespace WebShop.Domain.Entities;

public class Order : IEntity
{
    [Key]
    public int Id { get; }
    [Required]
    public Customer Customer { get; set; }
    [JsonIgnore]
    public ICollection<OrderProducts>? Products { get; set; }
}