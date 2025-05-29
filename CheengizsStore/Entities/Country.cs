namespace CheengizsStore.Entities;

public class Country
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public List<Sneaker> Sneakers { get; set; }
}