using CheengizsStore.DatabaseContexts;
using CheengizsStore.Entities;
using Microsoft.EntityFrameworkCore;
using CheengizsStore.RequestDTOs;

namespace CheengizsStore.Controllers;

public static class ModelsEndpoints
{
    public static RouteGroupBuilder MapModelsEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/models", async (StoreDbContext dbContext) =>
        {
            try
            {
                var models = new List<Model>();
                models.AddRange(await dbContext.Models.ToListAsync());
                return Results.Ok(models);
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        });

        group.MapGet("/models/{id:int}", async (StoreDbContext dbContext, int id) =>
        {
            try
            {
                var model = await dbContext.Models.FindAsync(id);
                if (model is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(model);
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        });

        group.MapPost("/models/{name}", async (StoreDbContext dbContext, ModelRequestDTO dto) =>
        {
            try
            {
                if (await dbContext.Models.AnyAsync(m => m.Name == dto.Name))
                {
                    return Results.Conflict("Model with the same name already exists");
                }

                var model = new Model()
                {
                    Name = dto.Name,
                    BrandId = dto.BrandId,
                };
                await dbContext.Models.AddAsync(model);
                await dbContext.SaveChangesAsync();
                return Results.Created("/api/v1/models/{id}", new { id = model.Id, name = model.Name });
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        });

        group.MapDelete("/models/{id}", async (StoreDbContext dbContext, int id) =>
        {
            try
            {
                var model = await dbContext.Models.FindAsync(id);
                if (model is null)
                {
                    return Results.NotFound();
                }

                dbContext.Models.Remove(model);
                await dbContext.SaveChangesAsync();
                return Results.NoContent();
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        });

        group.MapPost("/models/createTrial", async (StoreDbContext dbContext) =>
        {
            try
            {
                if (await dbContext.Models.AnyAsync())
                {
                    return Results.Conflict("Models was created");
                }

                var models = new List<Model>()
                {
                    new Model() { Name = "Initiator", BrandId = 2 },
                    new Model() { Name = "Air Force 1 '07", BrandId = 2 },
                    new Model() { Name = "Nike Air Max SC", BrandId = 2 },
                    new Model() { Name = "Blazer Mid '77", BrandId = 2 },
                    new Model() { Name = "Jordan 1 Retro High OG \"UNC Reimagined\"", BrandId = 2 },
                    new Model() { Name = "Speedcat OG", BrandId = 3 },
                    new Model() { Name = "Mayze Leather", BrandId = 3 },
                    new Model() { Name = "Suede Classic Sneakers Toddler", BrandId = 3 },
                    new Model() { Name = "GEL-KAYANO 14 ", BrandId = 4 },
                    new Model() { Name = "GEL-NYC 2055", BrandId = 4 },
                    new Model() { Name = "GEL-EXCITE 10", BrandId = 4 },
                    new Model() { Name = "GT-1000 13 PS ", BrandId = 4 },
                    new Model() { Name = "Samba OG Shoes", BrandId = 1 },
                    new Model() { Name = "Samba OG Shoes Kids", BrandId = 1 },
                    new Model() { Name = "530", BrandId = 5 },
                    new Model() { Name = "2002R ", BrandId = 5 },
                };
                
                await dbContext.Models.AddRangeAsync(models);
                await dbContext.SaveChangesAsync();

                return Results.Ok(models);
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        });

        return group;
    }
}