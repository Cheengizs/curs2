using CheengizsStore.DatabaseContexts;
using CheengizsStore.Entities;
using Microsoft.EntityFrameworkCore;

namespace CheengizsStore.Controllers;

public static class MaterialsEndpoints
{
    public static RouteGroupBuilder MapMaterialEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/materials", async (StoreDbContext dbContext) =>
        {
            try
            {
                var materials = new List<Material>();
                materials.AddRange(await dbContext.Materials.ToListAsync());
                return Results.Ok(materials);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }
        });

        group.MapGet("materials/{id}", async (StoreDbContext dbContext, int id) =>
        {
            try
            {
                var material = await dbContext.Materials.SingleOrDefaultAsync(e => e.Id == id);
                if (material is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(material);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e);
            }
        });

        group.MapPost("/materials/{name}", async (StoreDbContext dbContext, string name) =>
        {
            try
            {
                if (await dbContext.Materials.AnyAsync(e => e.Name == name))
                {
                    return Results.Conflict();
                }

                var material = new Material() { Name = name };
                await dbContext.Materials.AddAsync(material);
                await dbContext.SaveChangesAsync();
                return Results.Created($"/materials/{material.Id}", new { id = material.Id, name = material.Name });
            }
            catch (Exception e)
            {
                return Results.BadRequest(e);
            }
        });

        group.MapDelete("/materials/{id}", async (StoreDbContext dbContext, int id) =>
        {
            try
            {
                var material = await dbContext.Materials.SingleOrDefaultAsync(e => e.Id == id);
                if (material is null)
                {
                    return Results.NotFound();
                }

                dbContext.Materials.Remove(material);
                await dbContext.SaveChangesAsync();
                return Results.NoContent();
            }
            catch (Exception e)
            {
                return Results.BadRequest(e);
            }
        });

        group.MapPost("/materials/createTrial", async (StoreDbContext dbContext) =>
        {
            try
            {
                if (await dbContext.Materials.AnyAsync())
                {
                    return Results.Conflict("Materials already exists");
                }

                var materials = new List<Material>()
                {
                    new() { Name = "Кожа" },
                    new() { Name = "Дерматин" },
                    new() { Name = "Сетка" },
                    new() { Name = "Резина" },
                    new() { Name = "Пена" },
                    new() { Name = "Пластик" },
                };
                await dbContext.Materials.AddRangeAsync(materials);
                await dbContext.SaveChangesAsync();
                return Results.Created("/api/v1/materials", materials);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e);
            }
        });

        return group;
    }
}