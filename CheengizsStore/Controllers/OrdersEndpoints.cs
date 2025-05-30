using System.Security.Claims;
using System.Text;
using CheengizsStore.DatabaseContexts;
using CheengizsStore.Entities;
using CheengizsStore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CheengizsStore.Controllers;

public static class OrdersEndpoints
{
    public static RouteGroupBuilder MapOrdersEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/make-order", async (StoreDbContext dbContext, ClaimsPrincipal user, IEmailService emailDetka, [FromBody]string address) =>
        {
            try
            {
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Results.Unauthorized();

                var userId = Convert.ToInt32(userIdClaim.Value);

                var userCart = await dbContext.Carts
                    .Where(c => c.AccountId == userId)
                    .Include(c => c.SneakerProduct)
                    .ThenInclude(sp => sp.Stock).Include(cart => cart.SneakerProduct)
                    .ThenInclude(sneakerProduct => sneakerProduct.SneakerColor)
                    .ThenInclude(sneakerColor => sneakerColor.Sneaker).Include(cart => cart.SneakerProduct)
                    .ThenInclude(sneakerProduct => sneakerProduct.Size)
                    .ToListAsync();

                var hasExcess = userCart.Any(c => c.Amount > c.SneakerProduct.Stock.Amount);
                if (hasExcess)
                {
                    return Results.Conflict(new
                        { error = "Некоторые товары в корзине превышают наличие на складе." });
                }

                var orders = new List<Order>();

                StringBuilder msg = new StringBuilder();
                msg.Append("Модник, спасибо за заказ)) \n");
                foreach (var cartItem in userCart)
                {
                    var order = new Order
                    {
                        AccountId = userId,
                        SneakerProductId = cartItem.SneakerProductId,
                        Amount = cartItem.Amount,
                        Address = address,
                        IsComplete = false,
                        CreatedAt = DateTime.UtcNow,
                        TotalPrice = cartItem.SneakerProduct.SneakerColor.Price * cartItem.Amount,
                        OrderDisclaimer = "ничего"
                    };

                    cartItem.SneakerProduct.Stock.Amount -= cartItem.Amount;
                    msg.Append(
                        $"{order.Amount}x - {cartItem.SneakerProduct.SneakerColor.Sneaker.Name} " +
                        $"({cartItem.SneakerProduct.SneakerColor.Coloration}, размер {cartItem.SneakerProduct.Size.RusSize}) " +
                        $"с общей ценой {order.TotalPrice}");
                    orders.Add(order);
                }

                dbContext.Orders.AddRange(orders);

                dbContext.Carts.RemoveRange(userCart);

                await dbContext.SaveChangesAsync();

                var emailClaim = user.FindFirst(ClaimTypes.Email);

                emailDetka.SendEmailAsync(emailClaim.Value, "Модник, спасибо за покупку", msg.ToString());
                return Results.Ok(new { message = "Заказ успешно оформлен" });
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        }).RequireAuthorization();

        return group;
    }
}