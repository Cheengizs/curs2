using CheengizsStore.DatabaseContexts;
using CheengizsStore.Entities;
using CheengizsStore.RequestDTOs;
using Microsoft.EntityFrameworkCore;

namespace CheengizsStore.Controllers;

public static class SneakerToSalesEndpoints
{
    public static RouteGroupBuilder MapSneakerToSalesEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("", async (StoreDbContext dbContext) =>
        {
            try
            {
                var sneakerToSales = new List<SneakerToSale>();
                sneakerToSales.AddRange(await dbContext.SneakerToSales.ToListAsync());
                return Results.Ok(sneakerToSales);
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
                var sneakerToSale = await dbContext.SneakerToSales.FindAsync(id);
                if (sneakerToSale == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(sneakerToSale);
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        });

        group.MapPost("", async (StoreDbContext dbContext, SneakerToSaleRequestDTO dto) =>
        {
            try
            {
                var sneakerToSale = await dbContext.SneakerToSales.FirstOrDefaultAsync(s =>
                    s.SneakerProductId == dto.SneakerProductId && s.Price == dto.Price);
                if (sneakerToSale is not null)
                {
                    return Results.Conflict("SneakerToSale already exists");
                }

                sneakerToSale = new SneakerToSale()
                {
                    SneakerProductId = dto.SneakerProductId,
                    Price = dto.Price,
                };

                await dbContext.SneakerToSales.AddAsync(sneakerToSale);
                await dbContext.SaveChangesAsync();
                return Results.Ok(sneakerToSale);
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        });

        group.MapDelete("/{id}", async (StoreDbContext dbContext, int id) =>
        {
            try
            {
                var sneakerToSale = await dbContext.SneakerToSales.FindAsync(id);
                if (sneakerToSale is null)
                {
                    return Results.NotFound();
                }

                dbContext.SneakerToSales.Remove(sneakerToSale);
                await dbContext.SaveChangesAsync();
                return Results.NoContent();
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        });
        group.MapPost("/createTrial", async (StoreDbContext dbContext) =>
        {
            try
            {
                if (await dbContext.SneakerToSales.AnyAsync())
                {
                    return Results.Conflict("SneakerToSales already exists");
                }

                var sneakerToSales = new List<SneakerToSale>()
                {
                    new SneakerToSale()
                    {
                        SneakerProductId = 1,
                        Price = 600,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 2,
                        Price = (decimal)600.5,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 3,
                        Price = (decimal)640,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 4,
                        Price = (decimal)450,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 5,
                        Price = (decimal)660.5,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 6,
                        Price = (decimal)900.5,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 7,
                        Price = (decimal)1200,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 8,
                        Price = (decimal)800.5,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 9,
                        Price = (decimal)900,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 10,
                        Price = (decimal)1000,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 11,
                        Price = (decimal)500,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 12,
                        Price = (decimal)120,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 13,
                        Price = (decimal)500,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 14,
                        Price = (decimal)678,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 15,
                        Price = (decimal)789,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 16,
                        Price = (decimal)899.5,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 17,
                        Price = (decimal)772.99,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 18,
                        Price = (decimal)228.5,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 19,
                        Price = (decimal)899.99,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 20,
                        Price = (decimal)782.23,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 21,
                        Price = (decimal)799,
                    },

                    new SneakerToSale()
                    {
                        SneakerProductId = 22,
                        Price = (decimal)712,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 23,
                        Price = (decimal)2150,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 24,
                        Price = (decimal)892,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 25,
                        Price = (decimal)1000,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 26,
                        Price = (decimal)678,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 27,
                        Price = (decimal)542.11,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 28,
                        Price = (decimal)556,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 29,
                        Price = (decimal)555,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 30,
                        Price = (decimal)718.5,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 31,
                        Price = (decimal)781.4,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 32,
                        Price = (decimal)777,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 33,
                        Price = (decimal)526.5,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 34,
                        Price = (decimal)1276.76,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 35,
                        Price = (decimal)1234.56,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 36,
                        Price = (decimal)162,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 37,
                        Price = (decimal)899,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 38,
                        Price = (decimal)566,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 39,
                        Price = (decimal)522,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 40,
                        Price = (decimal)139,
                    },
                    new SneakerToSale()
                    {
                        SneakerProductId = 41,
                        Price = (decimal)140,
                    },
                };
                await dbContext.SneakerToSales.AddRangeAsync(sneakerToSales);
                await dbContext.SaveChangesAsync();
                return Results.Created("/api/v1/sneaker-to-sales", sneakerToSales);
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        });
        
        return group;
    }
}