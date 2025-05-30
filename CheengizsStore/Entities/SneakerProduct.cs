namespace CheengizsStore.Entities;

public class SneakerProduct
{
    public int Id { get; set; }
    public int SneakerColorId { get; set; }
    public int SizeId { get; set; }
    
    public Size Size { get; set; }
    public SneakerColor SneakerColor { get; set; }
    public Stock Stock { get; set; } 
    public List<Sale> Sales { get; set; }
    public List<Cart> Carts { get; set; }
    
    public List<Order> Orders { get; set; }
}