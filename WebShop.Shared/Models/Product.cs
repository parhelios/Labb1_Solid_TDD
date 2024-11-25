using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebShop.Shared.Interfaces;

namespace WebShop.Shared.Models;

public class Product : IProduct
{
    [Key]
    public int Id { get; init; }
    [Required(AllowEmptyStrings = false), MinLength(2)]
    public string Name { get; set; }
    [Required(AllowEmptyStrings = false), Range(0.00, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public double Price { get; set; }
    [Required(AllowEmptyStrings = false), DefaultValue(1), Range(0, int.MaxValue)]
    public int Amount { get; set; }
}