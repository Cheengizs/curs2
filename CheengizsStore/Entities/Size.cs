namespace CheengizsStore.Entities;

public class Size
{
    public int Id { get; set; }
    public decimal UsSize { get; set; }
    public decimal UkSize { get; set; }
    public decimal RusSize { get; set; }
    
    public List<SneakerProduct> SneakerProducts { get; set; }
}