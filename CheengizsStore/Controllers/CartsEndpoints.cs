using System.Security.Claims;
using CheengizsStore.DatabaseContexts;
using CheengizsStore.Entities;
using CheengizsStore.RequestDTOs;
using Microsoft.EntityFrameworkCore;

namespace CheengizsStore.Controllers;

public static class CartsEndpoints
{
    public static RouteGroupBuilder MapCartsEndpoints(this RouteGroupBuilder group)
    {
        group.MapDelete("/{id}", async (StoreDbContext dbContext, int id) =>
        {
            try
            {
                var cart = await dbContext.Carts.FindAsync(id);
                if (cart is null)
                {
                    return Results.NotFound("Cart not found");
                }
                
                dbContext.Carts.Remove(cart);
                await dbContext.SaveChangesAsync();
                return Results.NoContent();
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        }).RequireAuthorization();

        group.MapDelete("/delete/all", async (StoreDbContext dbContext, ClaimsPrincipal user) =>
        {
            try
            {
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Results.Unauthorized();

                var userId = Convert.ToInt32(userIdClaim.Value);

                
                var allUserCart = await dbContext.Carts.Select(s => s).Where(c => c.AccountId == userId).ToListAsync();
                
                dbContext.Carts.RemoveRange(allUserCart);
                await dbContext.SaveChangesAsync();
                return Results.NoContent();
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        }).RequireAuthorization();
        
        group.MapGet("", async (StoreDbContext dbContext, ClaimsPrincipal user) =>
        {
            try
            {
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Results.Unauthorized();

                var userId = Convert.ToInt32(userIdClaim.Value);

                var result = await dbContext.Carts
                    .Where(c => c.AccountId == userId)
                    .Select(c => new
                    {
                        cartId = c.Id,
                        sneakerColorId = c.SneakerProduct.SneakerColor.Id,
                        nameWithColoration = c.SneakerProduct.SneakerColor.Sneaker.Name + " (" +
                                             c.SneakerProduct.SneakerColor.Coloration + ")",
                        pricePerPair = c.SneakerProduct.SneakerColor.Price,
                        amount = c.Amount, // количество в этой записи корзины
                        totalPrice = c.Amount * c.SneakerProduct.SneakerColor.Price,
                        photoPath = c.SneakerProduct.SneakerColor.SneakerPhotos.Select(p => p.PhotoPath)
                            .FirstOrDefault() ?? "photos/noPhoto.png"
                    })
                    .ToListAsync();

                return Results.Ok(result);
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        }).RequireAuthorization();

        group.MapPost("", async (StoreDbContext dbContext, OrderRequestDTO dto, ClaimsPrincipal user) =>
        {
            try
            {
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Results.Unauthorized();

                var userId = Convert.ToInt32(userIdClaim.Value);

                var cart = await dbContext.Carts.FirstOrDefaultAsync(c =>
                    c.SneakerProduct.SneakerColorId == dto.SneakerColorId && c.AccountId == userId);
                if (cart != null)
                {
                    return Results.Conflict("В вашей корзине уже есть такой товар");
                }

                var newCart = new Cart()
                {
                    AccountId = userId,
                    Amount = dto.Amount,
                    SneakerProductId = dbContext.SneakerProducts.FirstOrDefault(sp =>
                        sp.SneakerColorId == dto.SneakerColorId && sp.SizeId == dto.SizeId).Id,
                };

                await dbContext.Carts.AddAsync(newCart);
                await dbContext.SaveChangesAsync();
                return Results.Created("/api/v1/cart", newCart);
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        }).RequireAuthorization();


        return group;
    }
}