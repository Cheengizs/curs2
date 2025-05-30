using System.Security.Claims;
using CheengizsStore.DatabaseContexts;
using Microsoft.EntityFrameworkCore;

namespace CheengizsStore.Controllers;

public static class AccountsEndpoints
{
    public static RouteGroupBuilder MapAccountsEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("", async (StoreDbContext dbContext, ClaimsPrincipal claims) =>
        {
            try
            {
                // Получаем имя пользователя из клейма (обычно так сохраняют Login или UserId)
                var login = claims.Identity?.Name;

                if (string.IsNullOrEmpty(login))
                {
                    return Results.Unauthorized();
                }

                var user = await dbContext.Accounts
                    .FirstOrDefaultAsync(a => a.Login == login);

                if (user == null)
                {
                    return Results.NotFound(new { error = "Пользователь не найден" });
                }

                return Results.Ok(new
                {
                    login = user.Login,
                    email = user.Email,
                    createdAt = user.CreatedAt
                });
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        }).RequireAuthorization();

        return group;
    }
}