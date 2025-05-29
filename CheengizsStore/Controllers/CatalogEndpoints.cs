using CheengizsStore.DatabaseContexts;
using CheengizsStore.FilterEntity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CheengizsStore.Controllers;

public static class CatalogEndpoints
{
    public static RouteGroupBuilder MapCatalogEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("", async (
            StoreDbContext dbContext,
            [FromQuery(Name = "brandId")] int[]? brandId,
            [FromQuery(Name = "typeId")] int[]? typeId,
            [FromQuery(Name = "colorId")] int[]? colorId,
            [FromQuery(Name = "materialId")] int[]? materialId,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize,
            [FromQuery] string? orderBy,
            [FromQuery] bool? inStock) =>
        {
            try
            {
                var filter = new FilterDTO()
                {
                    BrandIds = brandId,
                    TypeIds = typeId,
                    ColorIds = colorId,
                    MaterialIds = materialId,
                    MinPrice = minPrice,
                    MaxPrice = maxPrice,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    InStock = inStock ?? false,
                };

                var query = dbContext.SneakerToSales
                    .Include(s => s.SneakerProduct)
                    .ThenInclude(sp => sp.SneakerColor)
                    .ThenInclude(sc => sc.Sneaker)
                    .AsQueryable();

                // Фильтрация
                if (filter.BrandIds != null && filter.BrandIds.Any())
                    query = query.Where(
                        x => filter.BrandIds.Contains(x.SneakerProduct.SneakerColor.Sneaker.Model.BrandId));

                if (filter.TypeIds != null && filter.TypeIds.Any())
                    query = query.Where(
                        x => filter.TypeIds.Contains(x.SneakerProduct.SneakerColor.Sneaker.SneakerTypeId));

                if (filter.ColorIds != null && filter.ColorIds.Any())
                    query = query.Where(x =>
                        x.SneakerProduct.SneakerColor.Colors.Any(c => filter.ColorIds.Contains(c.Id)));

                if (filter.MaterialIds != null && filter.MaterialIds.Any())
                    query = query.Where(x =>
                        x.SneakerProduct.SneakerColor.Sneaker.Materials.Any(m => filter.MaterialIds.Contains(m.Id)));

                if (filter.MinPrice.HasValue)
                    query = query.Where(x => x.Price >= filter.MinPrice.Value);

                if (filter.MaxPrice.HasValue)
                    query = query.Where(x => x.Price <= filter.MaxPrice.Value);

                if (filter.InStock)
                    query = query.Where(x => x.SneakerProduct.Stock.Amount > 0);

                // Сортировка
                query = filter.OrderBy switch
                {
                    "lowPrice" => query.OrderBy(x => x.Price),
                    "highPrice" => query.OrderByDescending(x => x.Price),
                    "newest" => query.OrderByDescending(x => x.SneakerProduct.Id),
                    _ => query
                };

                // Пагинация
                int page = filter.PageNumber ?? 1;
                int pageSizePag = filter.PageSize ?? 12;
                var pagedResult = await query
                    .Skip((page - 1) * pageSizePag)
                    .Take(pageSizePag)
                    .Select(x => new
                    {
                        id = x.Id,
                        price = x.Price,
                        name = string.Concat(x.SneakerProduct.SneakerColor.Sneaker.Name, "(",
                            x.SneakerProduct.SneakerColor.Coloration, ")"),
                        photoPath = x.SneakerProduct.SneakerColor.SneakerPhotos
                            .Select(p => p.PhotoPath)
                            .FirstOrDefault() ?? "photos/noPhoto.png",
                    }).ToListAsync();
                return Results.Ok(pagedResult);
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        });


        return group;
    }
}