using System.ComponentModel.DataAnnotations;
using WebShop.Shared.Models.Interfaces;

namespace WebShop.Shared.Models;

public class Product : IProduct
{
    [Key]
    public int Id { get; }
    public string Name { get; set; }
    public double Price { get; set; }
    public int Amount { get; set; }
}