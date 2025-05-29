namespace CheengizsStore.Entities;

public class Sale
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public decimal Price { get; set; }
    public int SneakerProductId { get; set; }
    public int AccountId { get; set; }
    
    public SneakerProduct SneakerProduct { get; set; }
    public Account Account { get; set; }
}