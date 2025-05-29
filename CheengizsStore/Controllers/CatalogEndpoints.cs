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

                var query = dbContext.SneakerColors
                    .Include(sc => sc.Sneaker)
                    .AsQueryable();

                // Фильтрация
                if (filter.BrandIds != null && filter.BrandIds.Any())
                    query = query.Where(
                        x => filter.BrandIds.Contains(x.Sneaker.Model.BrandId));

                if (filter.TypeIds != null && filter.TypeIds.Any())
                    query = query.Where(
                        x => filter.TypeIds.Contains(x.Sneaker.SneakerTypeId));

                if (filter.ColorIds != null && filter.ColorIds.Any())
                    query = query.Where(x =>
                        x.Colors.Any(c => filter.ColorIds.Contains(c.Id)));

                if (filter.MaterialIds != null && filter.MaterialIds.Any())
                    query = query.Where(x =>
                        x.Sneaker.Materials.Any(m => filter.MaterialIds.Contains(m.Id)));

                if (filter.MinPrice.HasValue)
                    query = query.Where(x => x.Price >= filter.MinPrice.Value);

                if (filter.MaxPrice.HasValue)
                    query = query.Where(x => x.Price <= filter.MaxPrice.Value);

                if (filter.InStock)
                    query = query.Where(x => x.SneakerProducts.Any(e => e.Stock.Amount > 0));

                // Сортировка
                query = filter.OrderBy switch
                {
                    "lowPrice" => query.OrderBy(x => x.Price),
                    "highPrice" => query.OrderByDescending(x => x.Price),
                    "newest" => query.OrderByDescending(x => x.Id),
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
                        name = string.Concat(x.Sneaker.Name, " (",
                            x.Coloration, ")"),
                        photoPath = x.SneakerPhotos
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

        group.MapGet("/{id}", async (StoreDbContext dbContext, int id) =>
        {
            try
            {
                var sneakerColor = await dbContext.SneakerColors
                    .Include(sc => sc.Sneaker)
                    .ThenInclude(s => s.SneakerType)
                    .Include(sc => sc.Colors)
                    .Include(sc => sc.SneakerPhotos)
                    .Include(sc => sc.SneakerProducts)
                    .ThenInclude(sp => sp.Size)
                    .Include(sc => sc.SneakerProducts)
                    .ThenInclude(sp => sp.Stock)
                    .Include(sc => sc.Sneaker)
                    .ThenInclude(s => s.Materials)
                    .Include(sc => sc.Sneaker)
                    .ThenInclude(s => s.Country)
                    .Include(sc => sc.Sneaker)
                    .ThenInclude(s => s.SneakerColors)
                    .ThenInclude(sc => sc.SneakerPhotos) // <-- вот это обязательно
                    .FirstOrDefaultAsync(sc => sc.Id == id);

                if (sneakerColor == null)
                {
                    return Results.NotFound();
                }

                var newObj = new
                {
                    name = sneakerColor.Sneaker.Name + " (" + sneakerColor.Coloration + ")",
                    type = sneakerColor.Sneaker.SneakerType.Name,
                    coloration = sneakerColor.Coloration,
                    colors = sneakerColor.Colors.Select(c => c.Name).ToList(),
                    materials = sneakerColor.Sneaker.Materials.Select(m => m.Name).ToList(),
                    price = sneakerColor.Price,
                    year = sneakerColor.Sneaker.YearOfManufacture,
                    isActive = sneakerColor.Sneaker.IsActive,
                    country = sneakerColor.Sneaker.Country.Name,
                    photos = sneakerColor.SneakerPhotos.Select(p => p.PhotoPath).ToList(),
                    sizes = sneakerColor.SneakerProducts.Select(sp => new
                    {
                        rus_size = sp.Size.RusSize,
                        us_size = sp.Size.UsSize,
                        uk_size = sp.Size.UkSize,
                        amount = sp.Stock != null ? sp.Stock.Amount : 0
                    }).ToList(),
                    other = sneakerColor.Sneaker.SneakerColors.Select(e => new
                    {
                        id = e.Id,
                        photoPath = e.SneakerPhotos?.Select(p => p.PhotoPath).FirstOrDefault() ?? "photos/noPhoto.png"
                    }).ToList(),
                };
                
                return Results.Ok(newObj);
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        });


        return group;
    }
}