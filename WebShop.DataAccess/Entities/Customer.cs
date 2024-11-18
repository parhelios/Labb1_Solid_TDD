namespace WebShop.Entities;

public class Customer : IEntity
{
    public int Id { get; }
    public string Name { get; set; }
    public string Email { get; set; }
}