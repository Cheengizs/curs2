using System.Text.Json.Serialization;

namespace CheengizsStore.Entities;

public class Color
{
    public int Id { get; set; }
    public string Name { get; set; }
    [JsonIgnore]
    public List<SneakerColor> SneakerColors { get; set; }
}