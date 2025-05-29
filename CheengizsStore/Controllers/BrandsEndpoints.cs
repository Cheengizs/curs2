using System.Text.RegularExpressions;
using CheengizsStore.DatabaseContexts;
using CheengizsStore.Entities;
using Microsoft.EntityFrameworkCore;

namespace CheengizsStore.Controllers;

public static class BrandsEndpoints
{
    public static RouteGroupBuilder MapBrandsEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/brands", async (StoreDbContext dbContext) =>
        {
            try
            {
                var brands = new List<Brand>();
                brands.AddRange(await dbContext.Brands.ToListAsync());
                return Results.Ok(brands);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e);
            }
        });

        group.MapGet("/brands/{id}", async (StoreDbContext dbContext, int id) =>
        {
            try
            {
                var brand = await dbContext.Brands.SingleOrDefaultAsync(e => e.Id == id);
                if (brand is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(brand);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e);
            }
        });

        group.MapPost("/brands/{newBrand}", async (StoreDbContext dbContext, string newBrand) =>
        {
            try
            {
                if (dbContext.Brands.Any(e => e.Name == newBrand))
                {
                    return Results.Conflict("This brand exists already");
                }

                var newBrandObj = new Brand() { Name = newBrand };
                await dbContext.Brands.AddAsync(newBrandObj);
                await dbContext.SaveChangesAsync();
                return Results.Created($"/brands/{newBrandObj.Name}",
                    new { id = newBrandObj.Id, name = newBrandObj.Name });
            }
            catch (Exception e)
            {
                return Results.BadRequest(e);
            }
        });

        group.MapDelete("/brands/{id}", async (StoreDbContext dbContext, int id) =>
        {
            try
            {
                var brand = await dbContext.Brands.SingleOrDefaultAsync(e => e.Id == id);
                if (brand is null)
                {
                    return Results.NotFound();
                }

                dbContext.Brands.Remove(brand);
                await dbContext.SaveChangesAsync();
                return Results.NoContent();
            }
            catch (Exception e)
            {
                return Results.BadRequest(e);
            }
        });

        group.MapPost("/brands/createTrial", async (StoreDbContext dbContext) =>
        {
            try
            {
                if (dbContext.Brands.Any())
                {
                    return Results.Conflict("Brands already exist");
                }

                var brands = new List<Brand>()
                {
                    new Brand() { Name = "Adidas" },
                    new Brand() { Name = "Nike" },
                    new Brand() { Name = "Puma" },
                    new Brand() { Name = "Asics" },
                    new Brand() { Name = "New Balance" }
                };

                dbContext.Brands.AddRange(brands);
                await dbContext.SaveChangesAsync();
                return Results.Ok(brands);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e);
            }
        });

        return group;
    }
}