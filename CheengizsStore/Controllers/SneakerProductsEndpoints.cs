using CheengizsStore.DatabaseContexts;
using CheengizsStore.Entities;
using CheengizsStore.RequestDTOs;
using Microsoft.EntityFrameworkCore;

namespace CheengizsStore.Controllers;

public static class SneakerProductsEndpoints
{
    public static RouteGroupBuilder MapSneakerProductsEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/sneaker-products", async (StoreDbContext dbContext) =>
        {
            try
            {
                var sneakerProducts = new List<SneakerProduct>();
                sneakerProducts.AddRange(await dbContext.SneakerProducts.ToListAsync());
                return Results.Ok(sneakerProducts);
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        });

        group.MapGet("/sneaker-products/{id}", async (StoreDbContext? dbContext, int id) =>
        {
            try
            {
                var sneakerProduct = await dbContext.SneakerProducts.FindAsync(id);
                if (sneakerProduct == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(sneakerProduct);
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        });

        group.MapPost("/sneaker-products", async (StoreDbContext dbContext, SneakerProductRequestDTO dto) =>
        {
            try
            {
                var exists = await dbContext.SneakerProducts
                    .Where(s => s.SneakerColorId == dto.SneakerColorId && s.SizeId == dto.SizeId).AnyAsync();
                if (exists)
                {
                    return Results.Conflict("Sneaker product already exists");
                }

                var sneakerProduct = new SneakerProduct()
                {
                    SneakerColorId = dto.SneakerColorId,
                    SizeId = dto.SizeId,
                };

                await dbContext.SneakerProducts.AddAsync(sneakerProduct);
                await dbContext.SaveChangesAsync();
                return Results.Created($"/api/v1/sneaker-products/{sneakerProduct.Id}",
                    new
                    {
                        id = sneakerProduct.Id,
                        sneakerColorId = sneakerProduct.SneakerColorId,
                        sizeId = sneakerProduct.SizeId
                    });
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        });

        group.MapDelete("/sneaker-products/{id}", async (StoreDbContext? dbContext, int id) =>
        {
            try
            {
                var sneakerProduct = await dbContext.SneakerProducts.FindAsync(id);
                if (sneakerProduct == null)
                {
                    return Results.NotFound();
                }

                dbContext.SneakerProducts.Remove(sneakerProduct);
                await dbContext.SaveChangesAsync();
                return Results.NoContent();
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        });

        group.MapPost("/sneaker-products/createTrial", async (StoreDbContext dbContext) =>
        {
            try
            {
                if (await dbContext.SneakerProducts.AnyAsync())
                {
                    return Results.Conflict("Sneaker products already exists");
                }

                var sneakerProducts = new List<SneakerProduct>()
                {
                    new()
                    {
                        SneakerColorId = 1,
                        SizeId = 1,
                    },
                    new()
                    {
                        SneakerColorId = 1,
                        SizeId = 4,
                    },
                    new()
                    {
                        SneakerColorId = 1,
                        SizeId = 5,
                    },
                    new()
                    {
                        SneakerColorId = 1,
                        SizeId = 6,
                    },
                    new()
                    {
                        SneakerColorId = 2,
                        SizeId = 7,
                    },
                    new()
                    {
                        SneakerColorId = 2,
                        SizeId = 9,
                    },
                    new()
                    {
                        SneakerColorId = 3,
                        SizeId = 13,
                    },
                    new()
                    {
                        SneakerColorId = 3,
                        SizeId = 14,
                    },
                    new()
                    {
                        SneakerColorId = 4,
                        SizeId = 18,
                    },
                    new()
                    {
                        SneakerColorId = 5,
                        SizeId = 2,
                    },
                    new()
                    {
                        SneakerColorId = 5,
                        SizeId = 16,
                    },
                    new()
                    {
                        SneakerColorId = 6,
                        SizeId = 13,
                    },
                    new()
                    {
                        SneakerColorId = 6,
                        SizeId = 14,
                    },
                    new()
                    {
                        SneakerColorId = 7,
                        SizeId = 7,
                    },
                    new()
                    {
                        SneakerColorId = 7,
                        SizeId = 8,
                    },
                    new()
                    {
                        SneakerColorId = 8,
                        SizeId = 8,
                    },
                    new()
                    {
                        SneakerColorId = 8,
                        SizeId = 9,
                    },
                    new()
                    {
                        SneakerColorId = 9,
                        SizeId = 9,
                    },
                    new()
                    {
                        SneakerColorId = 9,
                        SizeId = 10,
                    },
                    new()
                    {
                        SneakerColorId = 10,
                        SizeId = 10,
                    },
                    new()
                    {
                        SneakerColorId = 10,
                        SizeId = 11,
                    },
                    new()
                    {
                        SneakerColorId = 12,
                        SizeId = 13,
                    },
                    new()
                    {
                        SneakerColorId = 14,
                        SizeId = 14,
                    },
                    new()
                    {
                        SneakerColorId = 15,
                        SizeId = 3,
                    },
                    new()
                    {
                        SneakerColorId = 16,
                        SizeId = 5,
                    },
                    new()
                    {
                        SneakerColorId = 17,
                        SizeId = 19,
                    },
                    new()
                    {
                        SneakerColorId = 18,
                        SizeId = 5,
                    },
                    new()
                    {
                        SneakerColorId = 19,
                        SizeId = 5,
                    },
                    new()
                    {
                        SneakerColorId = 20,
                        SizeId = 7,
                    },
                    new()
                    {
                        SneakerColorId = 21,
                        SizeId = 8,
                    },
                    new()
                    {
                        SneakerColorId = 22,
                        SizeId = 5,
                    },
                    new()
                    {
                        SneakerColorId = 23,
                        SizeId = 4,
                    },
                    new()
                    {
                        SneakerColorId = 24,
                        SizeId = 9,
                    },
                    new()
                    {
                        SneakerColorId = 25,
                        SizeId = 10,
                    },
                    new()
                    {
                        SneakerColorId = 26,
                        SizeId = 11,
                    },
                    new()
                    {
                        SneakerColorId = 27,
                        SizeId = 10,
                    },
                    new()
                    {
                        SneakerColorId = 27,
                        SizeId = 17,
                    },
                    new()
                    {
                        SneakerColorId = 28,
                        SizeId = 12,
                    },
                    new()
                    {
                        SneakerColorId = 29,
                        SizeId = 10,
                    },
                    new()
                    {
                        SneakerColorId = 30,
                        SizeId = 12,
                    },
                    new()
                    {
                        SneakerColorId = 30,
                        SizeId = 2,
                    },
                };

                await dbContext.AddRangeAsync(sneakerProducts);
                await dbContext.SaveChangesAsync();
                return Results.Created("/api/v1/sneaker-colors", sneakerProducts);
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        });

        return group;
    }
}