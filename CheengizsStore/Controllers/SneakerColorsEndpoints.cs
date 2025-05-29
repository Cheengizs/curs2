using CheengizsStore.DatabaseContexts;
using CheengizsStore.Entities;
using CheengizsStore.RequestDTOs;
using Microsoft.EntityFrameworkCore;

namespace CheengizsStore.Controllers;

public static class SneakerColorsEndpoints
{
    public static RouteGroupBuilder MapSneakerColorsEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/sneaker-colors", async (StoreDbContext dbContext) =>
        {
            try
            {
                var sneakerColors = new List<SneakerColor>();
                sneakerColors.AddRange(await dbContext.SneakerColors.ToListAsync());
                return Results.Ok(sneakerColors);
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        });

        group.MapGet("/sneaker-colors/{id}", async (StoreDbContext dbContext, int id) =>
        {
            try
            {
                var sneakerColor = await dbContext.SneakerColors.FindAsync(id);
                if (sneakerColor is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(sneakerColor);
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        });

        group.MapPost("/sneaker-colors", async (StoreDbContext dbContext, SneakerColorRequestDTO dto) =>
        {
            try
            {
                var exists = await dbContext.SneakerColors.AnyAsync(e =>
                    e.Coloration == dto.Coloration && e.SneakerId == dto.SneakerId);

                if (exists)
                {
                    return Results.Conflict("Sneaker coloration already exists");
                }

                var colors = await dbContext.Colors.ToListAsync();

                var sneakerColor = new SneakerColor()
                {
                    Coloration = dto.Coloration,
                    SneakerId = dto.SneakerId,
                    Colors = colors.Where(e => dto.ColorIds.Contains(e.Id)).ToList(),
                };

                dbContext.SneakerColors.Add(sneakerColor);
                await dbContext.SaveChangesAsync();

                return Results.Created();
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        });

        group.MapDelete("/sneaker-colors/{id}", async (StoreDbContext dbContext, int id) =>
        {
            try
            {
                var sneakerColor = await dbContext.SneakerColors.FindAsync(id);
                if (sneakerColor is null)
                {
                    return Results.NotFound();
                }

                dbContext.SneakerColors.Remove(sneakerColor);
                await dbContext.SaveChangesAsync();
                return Results.NoContent();
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        });

        group.MapPost("/sneaker-colors/createTrial", async (StoreDbContext dbContext) =>
        {
            try
            {
                if (dbContext.SneakerColors.Any())
                {
                    return Results.Conflict("Sneaker-colors already exists");
                }

                var colors = await dbContext.Colors.ToListAsync();

                var sneakerColors = new List<SneakerColor>()
                {
                    new()
                    {
                        Coloration = "Чёрно-белый",
                        Colors = colors.Where(c => new[] { 1, 2 }.Contains(c.Id)).ToList(),
                        SneakerId = 1,
                    },
                    new()
                    {
                        Coloration = "Металически-красный",
                        Colors = colors.Where(c => new[] { 1, 2, 4, 6, 7 }.Contains(c.Id)).ToList(),
                        SneakerId = 1,
                    },
                    new()
                    {
                        Coloration = "Обсидиановый",
                        Colors = colors.Where(c => new[] { 1, 2, 8, 10 }.Contains(c.Id)).ToList(),
                        SneakerId = 1,
                    },
                    new()
                    {
                        Coloration = "Металически-красный",
                        Colors = colors.Where(c => new[] { 1, 2, 4, 6, 7 }.Contains(c.Id)).ToList(),
                        SneakerId = 1,
                    },
                    new()
                    {
                        Coloration = "Белый",
                        Colors = colors.Where(c => new[] { 1, }.Contains(c.Id)).ToList(),
                        SneakerId = 2,
                    },
                    new()
                    {
                        Coloration = "Черный",
                        Colors = colors.Where(c => new[] { 2 }.Contains(c.Id)).ToList(),
                        SneakerId = 2,
                    },
                    new()
                    {
                        Coloration = "Чёрно-белый",
                        Colors = colors.Where(c => new[] { 1, 2 }.Contains(c.Id)).ToList(),
                        SneakerId = 3,
                    },
                    new()
                    {
                        Coloration = "Белый саммит",
                        Colors = colors.Where(c => new[] { 1, 2, 4, 8 }.Contains(c.Id)).ToList(),
                        SneakerId = 3,
                    },
                    new()
                    {
                        Coloration = "Чёрно-белый",
                        Colors = colors.Where(c => new[] { 1, 2, 4 }.Contains(c.Id)).ToList(),
                        SneakerId = 4,
                    },
                    new()
                    {
                        Coloration = "Персиково-белый",
                        Colors = colors.Where(c => new[] { 1, 4, 11 }.Contains(c.Id)).ToList(),
                        SneakerId = 4,
                    },
                    new()
                    {
                        Coloration = "Тёмно-пудровый",
                        Colors = colors.Where(c => new[] { 1, 9, 12 }.Contains(c.Id)).ToList(),
                        SneakerId = 5,
                    },
                    new()
                    {
                        Coloration = "Кофейный",
                        Colors = colors.Where(c => new[] { 2, 5, 13 }.Contains(c.Id)).ToList(),
                        SneakerId = 6,
                    },
                    new()
                    {
                        Coloration = "Ярко-красный",
                        Colors = colors.Where(c => new[] { 1, 2, 7 }.Contains(c.Id)).ToList(),
                        SneakerId = 6,
                    },
                    new()
                    {
                        Coloration = "Чёрно-белый",
                        Colors = colors.Where(c => new[] { 1, 2 }.Contains(c.Id)).ToList(),
                        SneakerId = 7,
                    },
                    new()
                    {
                        Coloration = "Синий",
                        Colors = colors.Where(c => new[] { 2, 8, 14 }.Contains(c.Id)).ToList(),
                        SneakerId = 8,
                    },
                    new()
                    {
                        Coloration = "Устричный сервый",
                        Colors = colors.Where(c => new[] { 1, 2, 4, 6 }.Contains(c.Id)).ToList(),
                        SneakerId = 9,
                    },
                    new()
                    {
                        Coloration = "Дымчато-серый",
                        Colors = colors.Where(c => new[] { 1, 2, 5 }.Contains(c.Id)).ToList(),
                        SneakerId = 9,
                    },
                    new()
                    {
                        Coloration = "Кремовый",
                        Colors = colors.Where(c => new[] { 1, 2, 4, 5, 6, }.Contains(c.Id)).ToList(),
                        SneakerId = 10,
                    },
                    new()
                    {
                        Coloration = "Серый",
                        Colors = colors.Where(c => new[] { 1, 2, 4 }.Contains(c.Id)).ToList(),
                        SneakerId = 10,
                    },
                    new()
                    {
                        Coloration = "Светлый",
                        Colors = colors.Where(c => new[] { 1, 11, }.Contains(c.Id)).ToList(),
                        SneakerId = 11,
                    },
                    new()
                    {
                        Coloration = "Чёрно-белый",
                        Colors = colors.Where(c => new[] { 1, 2, }.Contains(c.Id)).ToList(),
                        SneakerId = 11,
                    },
                    new()
                    {
                        Coloration = "Сине-жёлтый",
                        Colors = colors.Where(c => new[] { 1, 9, 8, }.Contains(c.Id)).ToList(),
                        SneakerId = 12,
                    },
                    new()
                    {
                        Coloration = "Ветренный",
                        Colors = colors.Where(c => new[] { 1, 9, 5, }.Contains(c.Id)).ToList(),
                        SneakerId = 12,
                    },
                    new()
                    {
                        Coloration = "Облачно-белый",
                        Colors = colors.Where(c => new[] { 1, 2, 13, 5, }.Contains(c.Id)).ToList(),
                        SneakerId = 13,
                    },
                    new()
                    {
                        Coloration = "Красный лед",
                        Colors = colors.Where(c => new[] { 1, 7, 13, 5 }.Contains(c.Id)).ToList(),
                        SneakerId = 13,
                    },
                    new()
                    {
                        Coloration = "Полу Флеш Аква",
                        Colors = colors.Where(c => new[] { 1, 10, 5, 14 }.Contains(c.Id)).ToList(),
                        SneakerId = 14,
                    },
                    new()
                    {
                        Coloration = "Черный",
                        Colors = colors.Where(c => new[] { 1, 2, 13, }.Contains(c.Id)).ToList(),
                        SneakerId = 14,
                    },
                    new()
                    {
                        Coloration = "Серая тень",
                        Colors = colors.Where(c => new[] { 1, 2, 4, }.Contains(c.Id)).ToList(),
                        SneakerId = 15,
                    },
                    new()
                    {
                        Coloration = "Ангора",
                        Colors = colors.Where(c => new[] { 1, 8, 9, }.Contains(c.Id)).ToList(),
                        SneakerId = 15,
                    },
                    new()
                    {
                        Coloration = "Сумеречный душ",
                        Colors = colors.Where(c => new[] { 9, 15, 5, }.Contains(c.Id)).ToList(),
                        SneakerId = 16,
                    }

                };

                await dbContext.SneakerColors.AddRangeAsync(sneakerColors);
                await dbContext.SaveChangesAsync();
                return Results.Created("/api/v1/sneaker_colors", sneakerColors);
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        });

        return group;
    }
}