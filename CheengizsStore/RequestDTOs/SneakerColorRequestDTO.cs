namespace CheengizsStore.RequestDTOs;

public class SneakerColorRequestDTO
{
    public int SneakerId { get; set; }

    public string Coloration { get; set; }

    public List<int> ColorIds { get; set; }
}