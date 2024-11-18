namespace WebShop.Entities;

public class Product : IEntity
{
    public int Id { get; }
    public string Name { get; set; }
}