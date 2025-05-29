using CheengizsStore.DatabaseContexts;
using CheengizsStore.Entities;
using Microsoft.EntityFrameworkCore;

namespace CheengizsStore.Controllers;

public static class TypesEndpoints
{
    public static RouteGroupBuilder MapTypesEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/types", async (StoreDbContext dbContext) =>
        {
            try
            {
                var types = new List<SneakerType>();
                types.AddRange(await dbContext.SneakerTypes.ToListAsync());
                return Results.Ok(types);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e);
            }
        });

        group.MapGet("/types/{id}", async (StoreDbContext dbContext, int id) =>
        {
            try
            {
                var type = await dbContext.SneakerTypes.SingleOrDefaultAsync(e => e.Id == id);
                if (type is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(type);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });

        group.MapPost("/types/{newType}", async (StoreDbContext dbContext, string newType) =>
        {
            try
            {
                if (await dbContext.SneakerTypes.AnyAsync(t => t.Name == newType))
                {
                    return Results.Conflict("Type already exists");
                }

                var newTypeObj = new SneakerType() { Name = newType };
                await dbContext.SneakerTypes.AddAsync(newTypeObj);
                await dbContext.SaveChangesAsync();
                return Results.Created($"/api/v1/types/{newTypeObj.Id}",
                    new { id = newTypeObj.Id, name = newTypeObj.Name });
            }
            catch (Exception e)
            {
                return Results.BadRequest(e);
            }
        });

        group.MapDelete("/types/{id}", async (StoreDbContext dbContext, int id) =>
        {
            try
            {
                var type = await dbContext.SneakerTypes.SingleOrDefaultAsync(e => e.Id == id);
                if (type is null)
                {
                    return Results.NotFound();
                }

                dbContext.SneakerTypes.Remove(type);
                await dbContext.SaveChangesAsync();
                return Results.NoContent();
            }
            catch (Exception e)
            {
                return Results.BadRequest(e);
            }
        });

        group.MapPut("/types/createTrial", async (StoreDbContext dbContext) =>
        {
            try
            {
                if (await dbContext.SneakerTypes.AnyAsync())
                {
                    return Results.Conflict("Types already exists");
                }

                var types = new List<SneakerType>()
                {
                    new() { Name = "Мужской" },
                    new() { Name = "Женский" },
                    new() { Name = "Детский" }
                };
                await dbContext.SneakerTypes.AddRangeAsync(types);
                await dbContext.SaveChangesAsync();
                return Results.Ok(types);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });
        

        return group;
    }
}