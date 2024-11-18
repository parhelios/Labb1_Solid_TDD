namespace WebShop.Entities;

public class ProductEntity : IEntity
{
    public int Id { get; }
    public string Name { get; set; }
}