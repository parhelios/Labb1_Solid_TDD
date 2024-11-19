using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebShop.Shared.Models.Interfaces;

namespace WebShop.Shared.Models;

public class Product : IProduct
{
    [Key]
    public int Id { get; }
    [Required]
    public string Name { get; set; }
    [Required, Range(0.00, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public double Price { get; set; }
    [Required, DefaultValue(1)]
    public int Amount { get; set; }
}