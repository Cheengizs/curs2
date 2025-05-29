using CheengizsStore.DatabaseContexts;
using CheengizsStore.Entities;
using Microsoft.EntityFrameworkCore;
using CheengizsStore.RequestDTOs;

namespace CheengizsStore.Controllers;

public static class SneakersEndpoints
{
    public static RouteGroupBuilder MapSneakersEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/sneakers", async (StoreDbContext dbContext) =>
        {
            try
            {
                var sneakers = new List<Sneaker>();
                sneakers.AddRange(await dbContext.Sneakers.ToListAsync());
                return Results.Ok(sneakers);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e);
            }
        });

        group.MapGet("/sneakers/{id}", async (StoreDbContext dbContext, int id) =>
        {
            try
            {
                var sneaker = await dbContext.Sneakers.FindAsync(id);
                if (sneaker is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(sneaker);
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        });

        group.MapPost("/sneakers", async (StoreDbContext dbContext, SneakerRequestDTO dto) =>
        {
            try
            {
                if (await dbContext.Sneakers.AnyAsync(e => e.Name == dto.Name))
                {
                    return Results.Conflict("Sneaker already exists");
                }

                var materials = new List<Material>();
                materials.AddRange(await dbContext.Materials.Where(m => dto.MaterialIds.Contains(m.Id)).ToListAsync());

                var sneaker = new Sneaker()
                {
                    Name = dto.Name,
                    YearOfManufacture = dto.YearOfManufacture,
                    ModelId = dto.ModelId,
                    SneakerTypeId = dto.SneakerTypeId,
                    CountryId = dto.CountryId,
                    IsActive = dto.IsActive,
                    Materials = materials
                };

                await dbContext.Sneakers.AddAsync(sneaker);
                await dbContext.SaveChangesAsync();
                return Results.Created($"/api/v1/sneakers/{sneaker.Id}", new
                {
                    id = sneaker.Id, name = sneaker.Name, yearOfManufacture = sneaker.YearOfManufacture,
                    modelId = sneaker.ModelId, type = sneaker.SneakerTypeId, country = sneaker.CountryId
                });
            }
            catch (DbUpdateException e)
            {
                return Results.BadRequest("One or more related entities (e.g., Material, Model, etc.) do not exist.");
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });

        group.MapDelete("/sneakers/{id}", async (StoreDbContext dbContext, int id) =>
        {
            try
            {
                var sneaker = await dbContext.Sneakers.SingleOrDefaultAsync(e => e.Id == id);
                if (sneaker is null)
                {
                    return Results.NotFound();
                }

                dbContext.Sneakers.Remove(sneaker);
                await dbContext.SaveChangesAsync();
                return Results.NoContent();
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        });

        group.MapPost("/sneakers/createTrial", async (StoreDbContext dbContext) =>
        {
            try
            {
                if (await dbContext.Sneakers.AnyAsync())
                {
                    return Results.Conflict("Sneakers already exists");
                }

                var materials = await dbContext.Materials.ToListAsync();

                var sneakers = new List<Sneaker>()
                {
                    new()
                    {
                        Name = "Nike Initiator",
                        CountryId = 1,
                        IsActive = true,
                        ModelId = 5,
                        Materials =
                            materials.Where(m => new[] { 3, 5, 7 }.Contains(m.Id)).ToList(),
                        SneakerTypeId = 1,
                        YearOfManufacture = 2023,
                    },
                    new()
                    {
                        Name = "Nike Air Force 1 '07",
                        CountryId = 3,
                        IsActive = true,
                        ModelId = 6,
                        Materials =
                            materials.Where(m => new[] { 1, 4, 7 }.Contains(m.Id)).ToList(),
                        SneakerTypeId = 1,
                        YearOfManufacture = 2007,
                    },
                    new()
                    {
                        Name = "Nike Air Max SC",
                        CountryId = 1,
                        IsActive = true,
                        ModelId = 7,
                        Materials = materials.Where(m => new[] { 1, 3, 4, 5, 8, }.Contains(m.Id))
                            .ToList(),
                        SneakerTypeId = 1,
                        YearOfManufacture = 2021,
                    },
                    new()
                    {
                        Name = "Nike Blazer Mid '77",
                        CountryId = 4,
                        IsActive = true,
                        ModelId = 8,
                        Materials = materials.Where(m => new[] { 1, 4, 5, 9, 10 }.Contains(m.Id))
                            .ToList(),
                        SneakerTypeId = 2,
                        YearOfManufacture = 2019,
                    },
                    new()
                    {
                        Name = "Nike Jordan 1 Retro High OG \"UNC Reimagined\"",
                        CountryId = 5,
                        IsActive = true,
                        ModelId = 9,
                        Materials = materials.Where(m => new[] { 1, 4, 5, 7 }.Contains(m.Id))
                            .ToList(),
                        SneakerTypeId = 3,
                        YearOfManufacture = 2025,
                    },
                    new()
                    {
                        Name = "Puma Speedcat OG",
                        CountryId = 4,
                        IsActive = true,
                        ModelId = 10,
                        Materials = materials.Where(m => new[] { 1, 9, 4, 7 }.Contains(m.Id))
                            .ToList(),
                        SneakerTypeId = 1,
                        YearOfManufacture = 2000,
                    },
                    new()
                    {
                        Name = "Puma Mayze Leather",
                        CountryId = 2,
                        IsActive = true,
                        ModelId = 11,
                        Materials = materials.Where(m => new[] { 1, 9, 4, 7 }.Contains(m.Id))
                            .ToList(),
                        SneakerTypeId = 2,
                        YearOfManufacture = 2021,
                    },
                    new()
                    {
                        Name = "PUMA x PLAYMOBIL Suede Classic Sneakers Toddler",
                        CountryId = 1,
                        IsActive = true,
                        ModelId = 12,
                        Materials = materials.Where(m => new[] { 9, 4, 11 }.Contains(m.Id))
                            .ToList(),
                        SneakerTypeId = 3,
                        YearOfManufacture = 2024,
                    },
                    new()
                    {
                        Name = "Asics GEL-KAYANO 14",
                        CountryId = 1,
                        IsActive = true,
                        ModelId = 13,
                        Materials = materials.Where(m => new[] { 3, 4, 5 }.Contains(m.Id))
                            .ToList(),
                        SneakerTypeId = 1,
                        YearOfManufacture = 2008,
                    },
                    new()
                    {
                        Name = "Asics GEL-NYC 2055",
                        CountryId = 4,
                        IsActive = true,
                        ModelId = 14,
                        Materials =
                            materials.Where(m => new[] { 3, 4, 5 }.Contains(m.Id)).ToList(),
                        SneakerTypeId = 1,
                        YearOfManufacture = 2024,
                    },
                    new()
                    {
                        Name = "Asics GEL-EXCITE 10",
                        CountryId = 3,
                        IsActive = false,
                        ModelId = 15,
                        Materials = materials.Where(m => new[] { 3, 5, 7 }.Contains(m.Id)).ToList(),
                        SneakerTypeId = 2,
                        YearOfManufacture = 2023,
                    },
                    new()
                    {
                        Name = "Asics GT-1000 13 PS",
                        CountryId = 3,
                        IsActive = true,
                        ModelId = 16,
                        Materials = materials.Where(m => new[] { 3, 4, 5, 11 }.Contains(m.Id))
                            .ToList(),
                        SneakerTypeId = 3,
                        YearOfManufacture = 2024,
                    },
                    new()
                    {
                        Name = "Adidas Samba OG Shoes",
                        CountryId = 4,
                        IsActive = true,
                        ModelId = 17,
                        Materials =
                            materials.Where(m => new[] { 1, 4, 9 }.Contains(m.Id)).ToList(),
                        SneakerTypeId = 1,
                        YearOfManufacture = 1949,
                    },
                    new()
                    {
                        Name = "Adidas Samba OG Shoes Kids",
                        CountryId = 5,
                        IsActive = false,
                        ModelId = 18,
                        Materials =
                            materials.Where(m => new[] { 1, 9, 4 }.Contains(m.Id)).ToList(),
                        SneakerTypeId = 3,
                        YearOfManufacture = 2023,
                    },
                    new()
                    {
                        Name = "New Balance 530",
                        CountryId = 5,
                        IsActive = false,
                        ModelId = 19,
                        Materials = materials.Where(m => new[] { 1, 3, 4, 9 }.Contains(m.Id)).ToList(),
                        SneakerTypeId = 1,
                        YearOfManufacture = 1992,
                    },
                    new()
                    {
                        Name = "New Balance 2002R ",
                        CountryId = 6,
                        IsActive = true,
                        ModelId = 20,
                        Materials = materials.Where(m => new[] { 1, 3, 4, 9 }.Contains(m.Id)).ToList(),
                        SneakerTypeId = 2,
                        YearOfManufacture = 2010,
                    },
                };

                await dbContext.Sneakers.AddRangeAsync(sneakers);
                await dbContext.SaveChangesAsync();
                return Results.Created();
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        });

        group.MapGet("/sneakers/1/1", async (StoreDbContext dbContext) =>
        {
            var result = await dbContext.Sneakers.Include(s => s.Materials).ToListAsync();

            return Results.Ok(result);
        });

        return group;
    }
}