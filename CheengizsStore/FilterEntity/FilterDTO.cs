using Microsoft.AspNetCore.Mvc;

namespace CheengizsStore.FilterEntity;

public class FilterDTO
{
    [FromQuery(Name = "brandId")] public int[]? BrandIds { get; set; }
    [FromQuery(Name = "typeId")] public int[]? TypeIds { get; set; }
    [FromQuery(Name = "colorId")] public int[]? ColorIds { get; set; }
    [FromQuery(Name = "materialId")] public int[]? MaterialIds { get; set; }
    [FromQuery] public decimal? MinPrice { get; set; }
    [FromQuery] public decimal? MaxPrice { get; set; }
    [FromQuery] public int? PageNumber { get; set; } = 1;
    [FromQuery] public int? PageSize { get; set; } = 12;
    [FromQuery] public string? OrderBy { get; set; }
    [FromQuery] public bool InStock { get; set; } = true;
}