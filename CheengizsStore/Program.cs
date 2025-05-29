using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using CheengizsStore.DatabaseContexts;
using CheengizsStore.AuthOptions;
using CheengizsStore.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using CheengizsStore.Controllers;


var builder = WebApplication.CreateBuilder(args);

SymmetricSecurityKey symmetricSecurityKey;
try
{
    symmetricSecurityKey =
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Secret-Key"]));
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}

builder.Services.AddDbContext<StoreDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"));
});

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    // options.SerializerOptions.WriteIndented = true;
});

builder.Services.AddAuthorization();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JwtConfig:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Secret-Key"]))
        };
    });


var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

app.MapPost("/api/v1/register", async (StoreDbContext dbContext, AuthOptions authOptions) =>
{
    if (authOptions.Login == "" || authOptions.Login.Length < 6)
        return Results.BadRequest("Невалидный логин");

    string passwRegex = @"^[a-zA-Z0-9!@#$%^&*()]+$";
    if (authOptions.Password == "" || authOptions.Password.Length < 8 ||
        !Regex.IsMatch(authOptions.Password, passwRegex))
        return Results.BadRequest("Невалидный пароль");

    var account = dbContext.Accounts.FirstOrDefault(acc => acc.Login == authOptions.Login);
    if (account != null)
        return Results.BadRequest("Аккаунт уже существует");

    account = new Account()
    {
        Login = authOptions.Login,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(authOptions.Password),
        CreatedAt = DateTime.UtcNow,
    };
    try
    {
        dbContext.Accounts.Add(account);
        dbContext.SaveChanges();
    }
    catch (Exception e)
    {
        return Results.BadRequest(e.Message);
    }

    return Results.Redirect("account/login");
});

app.MapPost("/api/v1/login", async Task<IResult> (StoreDbContext dbContext, AuthOptions authOptions) =>
{
    var account = dbContext.Accounts.FirstOrDefault(acc => acc.Login == authOptions.Login);

    if (account == null)
        return Results.BadRequest("Не найдено такого аккаунта");

    if (!BCrypt.Net.BCrypt.Verify(authOptions.Password, account.PasswordHash))
        return Results.Unauthorized();

    var claims = new List<Claim>() { new Claim(ClaimTypes.Name, account.Login) };

    var jwtToken = new JwtSecurityToken(
        issuer: builder.Configuration["JwtConfig:Issuer"],
        audience: builder.Configuration["JwtConfig:Audience"],
        claims: claims,
        expires: DateTime.Now.AddHours(1),
        signingCredentials: new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256)
    );

    return Results.Ok(new JwtSecurityTokenHandler().WriteToken(jwtToken));
});

app.MapGroup("/api/v1").MapColorEndpoints();

app.MapGroup("/api/v1").MapTypesEndpoints();

app.MapGroup("/api/v1").MapBrandsEndpoints();

app.MapGroup("/api/v1").MapModelsEndpoints();

app.MapGroup("/api/v1").MapMaterialEndpoints();

app.MapGroup("/api/v1").MapCountriesEndpoints();

app.MapGroup("/api/v1").MapSizesEndpoints();

app.MapGroup("/api/v1").MapSneakersEndpoints();

app.MapGroup("/api/v1").MapSneakerPhotosEndpoints();

app.MapGroup("/api/v1").MapSneakerColorsEndpoints();

app.MapGroup("/api/v1/").MapSneakerProductsEndpoints();

app.MapGroup("/api/v1/sneaker-photo").MapSneakerPhotosEndpoints();

app.MapGroup("/api/v1/catalog").MapCatalogEndpoints();

app.MapGroup("/api/v1/stocks").MapStocksEndpoints();

app.Run();