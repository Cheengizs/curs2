using CheengizsStore.DatabaseContexts;
using CheengizsStore.Entities;
using Microsoft.EntityFrameworkCore;

namespace CheengizsStore.Controllers;

public static class ColorsEndpoints
{
    public static RouteGroupBuilder MapColorEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/colors", async (StoreDbContext dbContext) =>
        {
            try
            {
                var colors = new List<Color>(); 
                colors.AddRange(dbContext.Colors.ToList());
                return Results.Ok(colors);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e);
            }
        });

        group.MapGet("/colors/{id}", async (StoreDbContext dbContext, int id) =>
        {
            try
            {
                var color = await dbContext.Colors.SingleOrDefaultAsync(e => e.Id == id);
                if (color is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(color);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e);
            }
        });

        group.MapPost("/colors/{newColor}", async (StoreDbContext dbContext, string newColor) =>
        {
            try
            {
                if (await dbContext.Colors.AnyAsync(c => c.Name == newColor))
                {
                    return Results.Conflict("Color already exists");
                }

                var newColorObj = new Color() { Name = newColor };
                await dbContext.Colors.AddAsync(newColorObj);
                await dbContext.SaveChangesAsync();
                return Results.Created($"/api/v1/colors/{newColorObj.Id}", new{id = newColorObj.Id, name = newColorObj.Name});
            }
            catch (Exception e)
            {
                return Results.BadRequest(e);
            }
        });

        group.MapDelete("/colors/{id}", async (StoreDbContext dbContext, int id) =>
        {
            try
            {
                var color = await dbContext.Colors.SingleOrDefaultAsync(c => c.Id == id);
                if (color is null)
                {
                    return Results.NotFound();
                }

                dbContext.Colors.Remove(color);
                await dbContext.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception e)
            {
                return Results.BadRequest(e);
            }
        });

        group.MapPost("/colors/createTrial", async (StoreDbContext dbContext) =>
        {
            if (dbContext.Colors.Any())
            {
                return Results.Ok("Colors was added already");
            }
            
            var colors = new List<Color>()
            {
                new() { Name = "Белый" },
                new() { Name = "Черный" },
                new() { Name = "Зеленый" },
                new() { Name = "Серый" },
                new() { Name = "Бежевый" }
            };
            await dbContext.Colors.AddRangeAsync(colors);
            await dbContext.SaveChangesAsync();
            return Results.Created($"/api/v1/colors", colors);
            
        });

        return group;
    }
}