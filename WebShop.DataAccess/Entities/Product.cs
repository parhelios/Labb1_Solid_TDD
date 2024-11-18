using System.ComponentModel.DataAnnotations;

namespace WebShop.DataAccess.Entities;

public class Product : IEntity
{
    [Key]
    public int Id { get; }
    public string Name { get; set; }
    public double Price { get; set; }
    public int Amount { get; set; }
}