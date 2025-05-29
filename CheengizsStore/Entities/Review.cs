namespace CheengizsStore.Entities;

public class Review
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Text { get; set; }
    public int AccountId { get; set; }
    public int SneakerId { get; set; }
    
    public Sneaker Sneaker { get; set; }
    public Account Account { get; set; }
}