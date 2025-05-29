namespace CheengizsStore.Entities;

public class SneakerToSale
{
    public int Id { get; set; }
    public int SneakerProductId { get; set; }
    public decimal Price { get; set; }
    
    public SneakerProduct SneakerProduct { get; set; }
    
    public List<Cart> Carts { get; set; }
}