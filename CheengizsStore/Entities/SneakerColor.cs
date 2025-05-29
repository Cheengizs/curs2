using CheengizsStore.Controllers;

namespace CheengizsStore.Entities;

public class SneakerColor
{
    public int Id { get; set; }
    public int SneakerId { get; set; }
    public string Coloration  { get; set; }
    
    public Sneaker Sneaker { get; set; }
    
    public List<Color> Colors { get; set; }
    public List<SneakerPhoto> SneakerPhotos { get; set; }
    public List<SneakerProduct> SneakerProducts { get; set; }
    public List<Order> Orders { get; set; }
    
}