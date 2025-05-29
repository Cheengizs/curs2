using CheengizsStore.DatabaseContexts;
using CheengizsStore.Entities;
using Microsoft.EntityFrameworkCore;

namespace CheengizsStore.Controllers;

public static class StocksEndpoints
{
    public static RouteGroupBuilder MapStocksEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("", async (StoreDbContext dbContext) =>
        {
            try
            {
                var stocks = new List<Stock>();
                stocks.AddRange(await dbContext.Stocks.ToListAsync());
                return Results.Ok(stocks);
            }
            catch (Exception e)
            {
                return Results.BadRequest(new{error = e.Message});
            }
        });
        
        group.MapPost("/createTrial", async (StoreDbContext dbContext) =>
        {
            try
            {
                if (await dbContext.Stocks.AnyAsync())
                {
                    return Results.Conflict();
                }
                
                foreach (var elem in await dbContext.SneakerProducts.ToListAsync())
                {
                    var rand = Random.Shared.Next(0, 7);
                    var st = new Stock()
                    {
                        Amount = rand - 4 > 0 ? rand - 4 : 0,
                        SneakerProductId = elem.Id
                    };
                    await dbContext.Stocks.AddAsync(st);
                }
                await dbContext.SaveChangesAsync();
                return Results.Created("/api/v1/stocks", dbContext.Stocks.ToList());
            }
            catch (Exception e)
            {
                return Results.BadRequest(new{error = e.Message});
            }
        });
        
        return group;
    }
}