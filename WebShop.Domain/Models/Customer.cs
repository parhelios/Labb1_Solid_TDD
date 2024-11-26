using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebShop.Domain.Interfaces;

namespace WebShop.Domain.Models;

public class Customer : IEntity
{
    [Key]
    public int Id { get; init; }
    [Required(AllowEmptyStrings = false), MinLength(2)]
    public string Name { get; set; }
    [Required(AllowEmptyStrings = false), EmailAddress]
    public string Email { get; set; }
    [JsonIgnore]
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}