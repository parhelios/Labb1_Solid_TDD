using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebShop.Shared.Models.Interfaces;

namespace WebShop.Shared.Models;

public class Order : IEntity
{
    [Key]
    public int Id { get; }
    [Required]
    public Customer Customer { get; set; }
    [JsonIgnore]
    public ICollection<OrderProducts>? Products { get; set; }
}