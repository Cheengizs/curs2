namespace CheengizsStore.Entities;

public class Stock
{
    public int Id { get; set; }
    public int Amount { get; set; }
    public int SneakerProductId { get; set; }
    
    public SneakerProduct SneakerProduct { get; set; }
}