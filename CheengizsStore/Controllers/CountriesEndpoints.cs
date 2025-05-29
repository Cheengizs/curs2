using CheengizsStore.DatabaseContexts;
using CheengizsStore.Entities;
using Microsoft.EntityFrameworkCore;

namespace CheengizsStore.Controllers;

public static class CountriesEndpoints
{
    public static RouteGroupBuilder MapCountriesEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/countries", async (StoreDbContext dbContext) =>
        {
            try
            {
                var countries = new List<Country>();
                countries.AddRange(dbContext.Countries.ToList());
                return Results.Ok(countries);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e);
            }
        });

        group.MapGet("/countries/{id}", async (StoreDbContext dbContext, int id) =>
        {
            try
            {
                var country = await dbContext.Countries.SingleOrDefaultAsync(e => e.Id == id);
                if (country is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(country);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e);
            }
        });

        group.MapPost("/countries/{name}", async (StoreDbContext dbContext, string name) =>
        {
            try
            {
                if (await dbContext.Countries.AnyAsync(c => c.Name == name))
                {
                    return Results.Conflict("Color already exists");
                }

                var country = new Country() { Name = name };
                await dbContext.Countries.AddAsync(country);
                await dbContext.SaveChangesAsync();
                return Results.Created($"/api/v1/colors/{country.Id}", new { id = country.Id, name = country.Name });
            }
            catch (Exception e)
            {
                return Results.BadRequest(e);
            }
        });

        group.MapDelete("/countries/{id}", async (StoreDbContext dbContext, int id) =>
        {
            try
            {
                var country = await dbContext.Countries.SingleOrDefaultAsync(c => c.Id == id);
                if (country is null)
                {
                    return Results.NotFound();
                }

                dbContext.Countries.Remove(country);
                await dbContext.SaveChangesAsync();

                return Results.NoContent();
            }
            catch (Exception e)
            {
                return Results.BadRequest(e);
            }
        });

        group.MapPost("/countries/createTrial", async (StoreDbContext dbContext) =>
        {
            if (dbContext.Countries.Any())
            {
                return Results.Ok("Countries was added already");
            }

            var countries = new List<Country>()
            {
                new() { Name = "Китай" },
                new() { Name = "Вьетнам" },
                new() { Name = "Индонезия" },
                new() { Name = "Индия" },
                new() { Name = "Бангладеш" }
            };
            await dbContext.Countries.AddRangeAsync(countries);
            await dbContext.SaveChangesAsync();
            return Results.Created($"/api/v1/countries", countries);
        });

        return group;
    }
}