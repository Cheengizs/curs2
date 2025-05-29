using CheengizsStore.DatabaseContexts;
using CheengizsStore.Entities;
using Microsoft.EntityFrameworkCore;

namespace CheengizsStore.Controllers;

public static class SizesEndpoints
{
    public static RouteGroupBuilder MapSizesEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/sizes", async (StoreDbContext dbContext) =>
        {
            try
            {
                var sizes = new List<Size>();
                sizes.AddRange(dbContext.Sizes.ToList());
                return Results.Ok(sizes);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e);
            }
        });

        group.MapGet("/sizes/{id}", async (StoreDbContext dbContext, int id) =>
        {
            try
            {
                var size = await dbContext.Countries.SingleOrDefaultAsync(e => e.Id == id);
                if (size is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(size);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e);
            }
        });

        group.MapPost("/sizes", async (StoreDbContext dbContext, HttpRequest httpRequest) =>
        {
            try
            {
                bool exists =
                    await dbContext.Sizes.AnyAsync(e => e.RusSize == Convert.ToDecimal(httpRequest.Query["rusSize"]));

                if (exists)
                {
                    return Results.Conflict("Size already exists");
                }

                var size = new Size()
                {
                    RusSize = Convert.ToDecimal(httpRequest.Query["rusSize"]),
                    UkSize = Convert.ToDecimal(httpRequest.Query["ukSize"]),
                    UsSize = Convert.ToDecimal(httpRequest.Query["usSize"]),
                };
                await dbContext.Sizes.AddAsync(size);
                await dbContext.SaveChangesAsync();
                return Results.Created();
            }
            catch (Exception e)
            {
                return Results.BadRequest(e);
            }
        });

        group.MapDelete("/sizes/{id}", async (StoreDbContext dbContext, int id) =>
        {
            try
            {
                var size = await dbContext.Sizes.SingleOrDefaultAsync(e => e.Id == id);
                if (size is null)
                {
                    return Results.NotFound();
                }

                dbContext.Sizes.Remove(size);
                await dbContext.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception e)
            {
                return Results.BadRequest(e);
            }
        });

        group.MapPost("/sizes/createTrial", async (StoreDbContext dbContext) =>
        {
            if (dbContext.Sizes.Any())
            {
                return Results.Ok("Countries was added already");
            }

            var sizes = new List<Size>()
            {
                new() { RusSize = (decimal)37, UkSize = (decimal)4.5, UsSize = (decimal)5 },
                new() { RusSize = (decimal)37.5, UkSize = (decimal)5, UsSize = (decimal)5.5 },
                new() { RusSize = (decimal)38, UkSize = (decimal)5.5, UsSize = (decimal)6 },
                new() { RusSize = (decimal)39, UkSize = (decimal)6, UsSize = (decimal)6.5 },
                new() { RusSize = (decimal)39.5, UkSize = (decimal)6.5, UsSize = (decimal)7 },
                new() { RusSize = (decimal)40, UkSize = (decimal)7, UsSize = (decimal)7.5 },
                new() { RusSize = (decimal)41, UkSize = (decimal)7.5, UsSize = (decimal)8 },
                new() { RusSize = (decimal)41.5, UkSize = (decimal)8, UsSize = (decimal)8.5 },
                new() { RusSize = (decimal)42, UkSize = (decimal)8.5, UsSize = (decimal)9 },
                new() { RusSize = (decimal)42.5, UkSize = (decimal)9, UsSize = (decimal)9.5 },
                new() { RusSize = (decimal)43, UkSize = (decimal)9.5, UsSize = (decimal)10 },
                new() { RusSize = (decimal)44, UkSize = (decimal)10, UsSize = (decimal)10.5 },
                new() { RusSize = (decimal)44.5, UkSize = (decimal)10.5, UsSize = (decimal)11 },
                new() { RusSize = (decimal)45, UkSize = (decimal)11, UsSize = (decimal)11.5 },
                new() { RusSize = (decimal)46, UkSize = (decimal)11.5, UsSize = (decimal)12 },
                new() { RusSize = (decimal)46.5, UkSize = (decimal)12, UsSize = (decimal)12.5 },
                new() { RusSize = (decimal)47, UkSize = (decimal)12.5, UsSize = (decimal)13 },
                new() { RusSize = (decimal)47.5, UkSize = (decimal)13, UsSize = (decimal)13.5 },
                new() { RusSize = (decimal)48, UkSize = (decimal)13.5, UsSize = (decimal)14 },
            };
            await dbContext.Sizes.AddRangeAsync(sizes);
            await dbContext.SaveChangesAsync();
            return Results.Created($"/api/v1/sizes", sizes);
        });

        return group;
    }
}