using System.Text.Json.Serialization;
using CheengizsStore.Entities;

namespace CheengizsStore.Controllers;

public class SneakerPhoto
{
    public int Id { get; set; }
    public string PhotoPath { get; set; }
    public int SneakerColorId { get; set; }
    
    [JsonIgnore]
    public SneakerColor SneakerColor { get; set; }
}