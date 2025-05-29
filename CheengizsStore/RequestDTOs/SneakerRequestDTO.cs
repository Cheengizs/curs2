namespace CheengizsStore.RequestDTOs;

public class SneakerRequestDTO
{
    public string Name { get; set; }
    public int YearOfManufacture { get; set; }
    public int ModelId { get; set; }
    public int SneakerTypeId { get; set; }
    public int CountryId { get; set; }
    public bool IsActive { get; set; }
    
    public List<int> MaterialIds { get; set; }
    
}