namespace CheengizsStore.Entities;

public class SneakerType
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public List<Sneaker> Sneakers { get; set; }
}