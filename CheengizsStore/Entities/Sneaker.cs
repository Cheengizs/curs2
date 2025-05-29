using System.Text.Json.Serialization;

namespace CheengizsStore.Entities;

public class Sneaker
{
    
    public int Id { get; set; }
    public string Name { get; set; }
    public int YearOfManufacture { get; set; }
    public int ModelId { get; set; }
    public int SneakerTypeId { get; set; }
    public int CountryId { get; set; }
    
    public bool IsActive { get; set; }
    
    public Country Country { get; set; }
    public SneakerType SneakerType { get; set; }
    public Model Model { get; set; }
    
    public List<Material> Materials { get; set; }
    public List<SneakerColor> SneakerColors { get; set; }
    public List<Review> Reviews { get; set; }
}