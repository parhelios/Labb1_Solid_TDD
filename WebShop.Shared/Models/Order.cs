namespace WebShop;

public class Order
{
    public int Id { get; }
    public Customer Customer { get; set; }
    public List<Product> Products { get; set; }
}