using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebShop.Shared.Models.Interfaces;

namespace WebShop.Shared.Models;

public class Customer : IEntity
{
    [Key]
    public int Id { get; }
    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; }
    [Required(AllowEmptyStrings = false), EmailAddress]
    public string Email { get; set; }
    [JsonIgnore]
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}