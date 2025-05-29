namespace CheengizsStore.Entities;

public class Account
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public List<Cart> Carts { get; set; }
    public List<Sale> Sales { get; set; }
    public List<Review> Reviews { get; set; }
    public List<Order> Orders { get; set; }
}