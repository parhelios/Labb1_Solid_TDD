namespace WebShop.Entities;

public class Order : IEntity
{
    public int Id { get; }
    public Customer Customer { get; set; }
    public List<Product> Products { get; set; }
}