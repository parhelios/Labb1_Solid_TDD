using System.ComponentModel.DataAnnotations;

namespace WebShop.Entities;

public class Customer : IEntity
{
    [Key]
    public int Id { get; }
    public string Name { get; set; }
    public string Email { get; set; }
}