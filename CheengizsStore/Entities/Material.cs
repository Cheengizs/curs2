using System.Text.Json.Serialization;

namespace CheengizsStore.Entities;

public class Material
{
    public int Id { get; set; }
    public string Name { get; set; }
    [JsonIgnore]
    public List<Sneaker> Sneakers { get; set; }
}