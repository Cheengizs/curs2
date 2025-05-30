namespace CheengizsStore.Entities;

public class Order
{
    public int Id { get; set; }
    public int Amount { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsComplete { get; set; }
    public string Address { get; set; }
    public string OrderDisclaimer { get; set; }
    public int AccountId { get; set; }
    public int SneakerProductId { get; set; }
    public SneakerProduct SneakerProduct { get; set; }
    public Account Account { get; set; }
}