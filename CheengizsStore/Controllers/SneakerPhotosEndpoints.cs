using CheengizsStore.DatabaseContexts;
using CheengizsStore.RequestDTOs;
using Microsoft.EntityFrameworkCore;

namespace CheengizsStore.Controllers;

public static class SneakerPhotosEndpoints
{
    public static RouteGroupBuilder MapSneakerPhotosEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/sneaker-photos", async (StoreDbContext dbContext) =>
        {
            try
            {
                var sneakerPhotos = new List<SneakerPhoto>();
                sneakerPhotos.AddRange(await dbContext.SneakerPhotos.ToListAsync());
                return Results.Ok(sneakerPhotos);
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        });

        group.MapGet("/sneaker-photos/{id}", async (StoreDbContext dbContext, int id) =>
        {
            try
            {
                var sneakerPhoto = await dbContext.SneakerPhotos.FindAsync(id);
                if (sneakerPhoto is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(sneakerPhoto);
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        });

        group.MapPost("/sneaker-photos", async (StoreDbContext dbContext, SneakerPhotoRequestDTO dto) =>
        {
            try
            {
                var sneakerPhoto = await dbContext.SneakerPhotos.FirstOrDefaultAsync(e => e.PhotoPath == dto.PhotoPath);
                if (sneakerPhoto is not null)
                {
                    return Results.Conflict(new { error = "Photo already exists" });
                }

                sneakerPhoto = new SneakerPhoto() { PhotoPath = dto.PhotoPath, SneakerColorId = dto.SneakerColorId };
                await dbContext.SneakerPhotos.AddAsync(sneakerPhoto);
                await dbContext.SaveChangesAsync();
                return Results.Created($"/api/v1/sneaker-photos/{sneakerPhoto.Id}",
                    new
                    {
                        id = sneakerPhoto.Id, photoPath = sneakerPhoto.PhotoPath,
                        snekaerColorId = sneakerPhoto.SneakerColorId
                    });
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        });

        group.MapDelete("/sneaker-photos/{id}", async (StoreDbContext dbContext, int id) =>
        {
            try
            {
                var sneakerPhoto = await dbContext.SneakerPhotos.FindAsync(id);
                if (sneakerPhoto is null)
                {
                    return Results.NotFound("Sneaker photo not found");
                }

                dbContext.SneakerPhotos.Remove(sneakerPhoto);
                await dbContext.SaveChangesAsync();
                return Results.NoContent();
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        });

        group.MapPost("/sneaker-photos/createTrial", async (StoreDbContext dbContext) =>
        {
            try
            {
                if (await dbContext.SneakerPhotos.AnyAsync())
                {
                    return Results.Conflict(new { error = "Photo already exists" });
                }

                var sneakerPhotos = new List<SneakerPhoto>()
                {
                    new()
                    {
                        SneakerColorId = 1,
                        PhotoPath = "photos/nike/men/nike_initiator_white_black_main.png",
                    },
                    new()
                    {
                        SneakerColorId = 1,
                        PhotoPath = "photos/nike/men/nike_initiator_white_black_top.png",
                    },
                    new()
                    {
                        SneakerColorId = 1,
                        PhotoPath = "photos/nike/men/nike_initiator_white_black.png",
                    },
                    new()
                    {
                        SneakerColorId = 2,
                        PhotoPath = "photos/nike/men/nike_initiator_red.png",
                    },
                    new()
                    {
                        SneakerColorId = 2,
                        PhotoPath = "photos/nike/men/nike_initiator_red_top.png",
                    },
                    new()
                    {
                        SneakerColorId = 2,
                        PhotoPath = "photos/nike/men/nike_initiator_red_notMain.png",
                    },
                    new()
                    {
                        SneakerColorId = 3,
                        PhotoPath = "photos/nike/men/nike_initiator_obsidian.png",
                    },
                    new()
                    {
                        SneakerColorId = 3,
                        PhotoPath = "photos/nike/men/nike_initiator_obsidian_top.png",
                    },
                    new()
                    {
                        SneakerColorId = 3,
                        PhotoPath = "photos/nike/men/nike_initiator_obsidian_notMain.png",
                    },
                    new()
                    {
                        SneakerColorId = 5,
                        PhotoPath = "photos/nike/men/nike_air_force.png",
                    },
                    new()
                    {
                        SneakerColorId = 5,
                        PhotoPath = "photos/nike/men/nike_air_force_top.png",
                    },
                    new()
                    {
                        SneakerColorId = 5,
                        PhotoPath = "photos/nike/men/nike_air_force_notMain.png",
                    },
                    new()
                    {
                        SneakerColorId = 6,
                        PhotoPath = "photos/nike/men/nike_air_force_black.png",
                    },
                    new()
                    {
                        SneakerColorId = 6,
                        PhotoPath = "photos/nike/men/nike_air_force_black_top.png",
                    },
                    new()
                    {
                        SneakerColorId = 6,
                        PhotoPath = "photos/nike/men/nike_air_force_black_notMain.png",
                    },
                    new()
                    {
                        SneakerColorId = 7,
                        PhotoPath = "photos/nike/men/nike_air_white.png",
                    },
                    new()
                    {
                        SneakerColorId = 7,
                        PhotoPath = "photos/nike/men/nike_air_white_top.png",
                    },
                    new()
                    {
                        SneakerColorId = 7,
                        PhotoPath = "photos/nike/men/nike_air_white_notMain.png",
                    },
                    new()
                    {
                        SneakerColorId = 8,
                        PhotoPath = "photos/nike/men/nike_air_summit.png",
                    },
                    new()
                    {
                        SneakerColorId = 8,
                        PhotoPath = "photos/nike/men/nike_air_summit_top.png",
                    },
                    new()
                    {
                        SneakerColorId = 8,
                        PhotoPath = "photos/nike/men/nike_air_summit_notMain.png",
                    },
                    new()
                    {
                        SneakerColorId = 9,
                        PhotoPath = "photos/nike/men/nike_blazer_white.png",
                    },
                    new()
                    {
                        SneakerColorId = 9,
                        PhotoPath = "photos/nike/men/nike_blazer_white_top.png",
                    },
                    new()
                    {
                        SneakerColorId = 9,
                        PhotoPath = "photos/nike/men/nike_blazer_white_notMain.png",
                    },
                    new()
                    {
                        SneakerColorId = 10,
                        PhotoPath = "photos/nike/men/nike_blazer_pink.png",
                    },
                    new()
                    {
                        SneakerColorId = 10,
                        PhotoPath = "photos/nike/men/nike_blazer_pink_top.png",
                    },
                    new()
                    {
                        SneakerColorId = 10,
                        PhotoPath = "photos/nike/men/nike_blazer_notMain.png",
                    },
                    new()
                    {
                        SneakerColorId = 11,
                        PhotoPath = "photos/nike/men/nike_jordan.png",
                    },
                    new()
                    {
                        SneakerColorId = 11,
                        PhotoPath = "photos/nike/men/nike_jordan_top.png",
                    },
                    new()
                    {
                        SneakerColorId = 11,
                        PhotoPath = "photos/nike/men/nike_jordan_notMain.png",
                    },
                    new()
                    {
                        SneakerColorId = 12,
                        PhotoPath = "photos/puma/puma_speedcat.png",
                    },
                    new()
                    {
                        SneakerColorId = 12,
                        PhotoPath = "photos/puma/puma_speedcat_top.png",
                    },
                    new()
                    {
                        SneakerColorId = 12,
                        PhotoPath = "photos/puma/puma_speedcat_notMain.png",
                    },
                    new()
                    {
                        SneakerColorId = 13,
                        PhotoPath = "photos/puma/puma_speedcat_red.png",
                    },
                    new()
                    {
                        SneakerColorId = 13,
                        PhotoPath = "photos/puma/puma_speedcat_red_top.png",
                    },
                    new()
                    {
                        SneakerColorId = 13,
                        PhotoPath = "photos/puma/puma_speedcat_red_notMain.png",
                    },
                    new()
                    {
                        SneakerColorId = 14,
                        PhotoPath = "photos/puma/mayze.png",
                    },
                    new()
                    {
                        SneakerColorId = 14,
                        PhotoPath = "photos/puma/mayze_top.png",
                    },
                    new()
                    {
                        SneakerColorId = 14,
                        PhotoPath = "photos/puma/mayze_notMain.png",
                    },
                    new()
                    {
                        SneakerColorId = 15,
                        PhotoPath = "photos/puma/playmobil.png",
                    },
                    new()
                    {
                        SneakerColorId = 15,
                        PhotoPath = "photos/puma/playmobil_top.png",
                    },
                    new()
                    {
                        SneakerColorId = 15,
                        PhotoPath = "photos/puma/playmobil_notMain.png",
                    },
                    new()
                    {
                        SneakerColorId = 16,
                        PhotoPath = "photos/asics/gray.png",
                    },
                    new()
                    {
                        SneakerColorId = 16,
                        PhotoPath = "photos/asics/gray_top.png",
                    },
                    new()
                    {
                        SneakerColorId = 16,
                        PhotoPath = "photos/asics/gray_notMain.png",
                    },
                    new()
                    {
                        SneakerColorId = 17,
                        PhotoPath = "photos/asics/white.png",
                    },
                    new()
                    {
                        SneakerColorId = 17,
                        PhotoPath = "photos/asics/white_top.png",
                    },
                    new()
                    {
                        SneakerColorId = 17,
                        PhotoPath = "photos/asics/white_notMain.png",
                    },
                    new()
                    {
                        SneakerColorId = 18,
                        PhotoPath = "photos/asics/cream.png",
                    },
                    new()
                    {
                        SneakerColorId = 18,
                        PhotoPath = "photos/asics/cream_top.png",
                    },
                    new()
                    {
                        SneakerColorId = 19,
                        PhotoPath = "photos/asics/blue.png",
                    },
                    new()
                    {
                        SneakerColorId = 19,
                        PhotoPath = "photos/asics/blue_top.png",
                    },
                    new()
                    {
                        SneakerColorId = 20,
                        PhotoPath = "photos/asics/pink.png",
                    },
                    new()
                    {
                        SneakerColorId = 20,
                        PhotoPath = "photos/asics/pink_top.png",
                    },
                    new()
                    {
                        SneakerColorId = 21,
                        PhotoPath = "photos/asics/black.png",
                    },
                    new()
                    {
                        SneakerColorId = 21,
                        PhotoPath = "photos/asics/black_top.png",
                    },
                    new()
                    {
                        SneakerColorId = 22,
                        PhotoPath = "photos/asics/kid.png",
                    },
                    new()
                    {
                        SneakerColorId = 22,
                        PhotoPath = "photos/asics/kid_top.png",
                    },
                    new()
                    {
                        SneakerColorId = 23,
                        PhotoPath = "photos/asics/breeze.png",
                    },
                    new()
                    {
                        SneakerColorId = 23,
                        PhotoPath = "photos/asics/breeze_top.png",
                    },
                    new()
                    {
                        SneakerColorId = 24,
                        PhotoPath = "photos/adidas/white.png",
                    },
                    new()
                    {
                        SneakerColorId = 24,
                        PhotoPath = "photos/adidas/white_top.png",
                    },
                    new()
                    {
                        SneakerColorId = 25,
                        PhotoPath = "photos/adidas/red.png",
                    },
                    new()
                    {
                        SneakerColorId = 25,
                        PhotoPath = "photos/adidas/red_top.png",
                    },
                    new()
                    {
                        SneakerColorId = 26,
                        PhotoPath = "photos/adidas/kid.png",
                    },
                    new()
                    {
                        SneakerColorId = 26,
                        PhotoPath = "photos/adidas/kid_top.png",
                    },
                    new()
                    {
                        SneakerColorId = 27,
                        PhotoPath = "photos/adidas/black.png",
                    },
                    new()
                    {
                        SneakerColorId = 27,
                        PhotoPath = "photos/adidas/black_top.png",
                    },
                    new()
                    {
                        SneakerColorId = 28,
                        PhotoPath = "photos/newBalnce/white.png",
                    },
                    new()
                    {
                        SneakerColorId = 28,
                        PhotoPath = "photos/newBalnce/white_top.png",
                    },
                    new()
                    {
                        SneakerColorId = 29,
                        PhotoPath = "photos/newBalance/angora.png",
                    },
                    new()
                    {
                        SneakerColorId = 29,
                        PhotoPath = "photos/newBalance/angora_top.png",
                    },
                    new()
                    {
                        SneakerColorId = 30,
                        PhotoPath = "photos/newBalance/gray.png",
                    },
                    new()
                    {
                        SneakerColorId = 30,
                        PhotoPath = "photos/newBalance/gray_top.png",
                    },
                    new()
                    {
                        SneakerColorId = 30,
                        PhotoPath = "photos/newBalance/gray_notMain.png",
                    },
                };

                await dbContext.SneakerPhotos.AddRangeAsync(sneakerPhotos);
                await dbContext.SaveChangesAsync();
                return Results.Created("/api/v1/sneaker-photos", sneakerPhotos);
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        });

        group.MapGet("/1/1", async () => "photos/nike/men/nike_initiator_red.png");

        return group;
    }
}