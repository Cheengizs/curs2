namespace CheengizsStore.Entities;

public class Cart
{
    public int Id { get; set; }
    public int Amount { get; set; }
    public int AccountId { get; set; }
    public int SneakerProductId { get; set; }
    
    public SneakerProduct SneakerProduct { get; set; }
    
    public Account Account { get; set; }
}