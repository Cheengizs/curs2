using CheengizsStore.Controllers;
using CheengizsStore.Entities;
using Microsoft.EntityFrameworkCore;

namespace CheengizsStore.DatabaseContexts;

public class StoreDbContext : DbContext
{
    public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<Model> Models { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<Size> Sizes { get; set; }
    public DbSet<Sneaker> Sneakers { get; set; }
    public DbSet<SneakerColor> SneakerColors { get; set; }
    public DbSet<SneakerPhoto> SneakerPhotos { get; set; }
    public DbSet<SneakerProduct> SneakerProducts { get; set; }
    public DbSet<SneakerType> SneakerTypes { get; set; }
    public DbSet<Stock> Stocks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(30).IsRequired();
            entity.HasIndex(e => e.Name).IsUnique();
        });

        modelBuilder.Entity<SneakerType>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(30).IsRequired();
            entity.HasIndex(e => e.Name).IsUnique();
        });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(30).IsRequired();
            entity.HasIndex(e => e.Name).IsUnique();
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(30).IsRequired();
            entity.HasIndex(e => e.Name).IsUnique();
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UkSize, e.RusSize, e.UsSize }).IsUnique();

            entity.Property(e => e.UsSize)
                .HasPrecision(3, 1)
                .IsRequired();

            entity.Property(e => e.UkSize)
                .HasPrecision(3, 1)
                .IsRequired();

            entity.Property(e => e.RusSize)
                .HasPrecision(3, 1)
                .IsRequired();
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(30).IsRequired();
            entity.HasIndex(e => e.Name).IsUnique();
        });

        modelBuilder.Entity<Model>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.HasIndex(e => e.Name).IsUnique();
            entity.HasIndex(e => e.BrandId);

            entity.HasOne(e => e.Brand).WithMany(b => b.Models).HasForeignKey(e => e.BrandId);
        });

        modelBuilder.Entity<Sneaker>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.YearOfManufacture).IsRequired();
            
            entity.HasOne(e => e.Country).WithMany(b => b.Sneakers).HasForeignKey(e => e.CountryId);
            entity.HasOne(e => e.SneakerType).WithMany(e => e.Sneakers).HasForeignKey(e => e.SneakerTypeId);
            entity.HasOne(e => e.Model).WithMany(b => b.Sneakers).HasForeignKey(e => e.ModelId);

            entity.HasMany(e => e.Materials).WithMany(e => e.Sneakers).UsingEntity(j => j.ToTable("SneakerMaterial"));
        });

        modelBuilder.Entity<SneakerColor>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.SneakerId);
            entity.HasOne(e => e.Sneaker).WithMany(e => e.SneakerColors).HasForeignKey(e => e.SneakerId);
            entity.HasMany(e => e.Colors).WithMany(e => e.SneakerColors);
            entity.Property(e => e.Coloration).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Price).HasPrecision(6, 2).IsRequired();
        });

        modelBuilder.Entity<SneakerPhoto>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.PhotoPath).IsRequired().HasMaxLength(500);
            entity.HasOne(e => e.SneakerColor).WithMany(e => e.SneakerPhotos).HasForeignKey(e => e.SneakerColorId); 
        });

        modelBuilder.Entity<SneakerProduct>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Size).WithMany(e => e.SneakerProducts).HasForeignKey(e => e.SizeId);
            entity.HasIndex(e => e.SizeId);
            entity.HasOne(e => e.SneakerColor).WithMany(e => e.SneakerProducts).HasForeignKey(e => e.SneakerColorId);
            entity.HasIndex(e => e.SneakerColorId);
            entity.HasOne(e => e.Stock).WithOne(e => e.SneakerProduct);
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).IsRequired();
            entity.HasIndex(e => e.SneakerProductId);
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Price).HasPrecision(6, 2).IsRequired();
            entity.HasOne(e => e.SneakerProduct).WithMany(e => e.Sales).HasForeignKey(e => e.SneakerProductId);
            entity.HasIndex(e => e.SneakerProductId);
            entity.HasOne(e => e.Account).WithMany(e => e.Sales).HasForeignKey(e => e.AccountId);
            entity.HasIndex(e => e.AccountId);
        });

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Login).HasMaxLength(20).IsRequired();
            entity.HasIndex(e => e.Login).IsUnique();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.PasswordHash).HasMaxLength(1000).IsRequired();
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).IsRequired();
            entity.HasOne(e => e.Account).WithMany(c => c.Carts).HasForeignKey(e => e.AccountId);
            entity.HasIndex(e => e.AccountId);
            entity.HasOne(e => e.SneakerProduct).WithMany(c => c.Carts).HasForeignKey(e => e.SneakerProductId);
            entity.HasIndex(e => e.SneakerProductId);
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.Text).HasMaxLength(500).IsRequired();
            entity.HasOne(e => e.Sneaker).WithMany(e => e.Reviews).HasForeignKey(e => e.SneakerId);
            entity.HasOne(e => e.Account).WithMany(e => e.Reviews).HasForeignKey(e => e.AccountId);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.HasIndex(e => e.CreatedAt);
            entity.Property(e => e.IsComplete).IsRequired();
            entity.Property(e => e.Address).HasMaxLength(200).IsRequired();
            entity.Property(e => e.OrderDisclaimer).HasMaxLength(300);
            entity.Property(e => e.TotalPrice).HasPrecision(6, 2).IsRequired();
            entity.HasOne(e => e.Account).WithMany(e => e.Orders).HasForeignKey(e => e.AccountId);
            entity.HasIndex(e => e.AccountId);
            entity.HasOne(e => e.SneakerColor).WithMany(e => e.Orders).HasForeignKey(e => e.SneakerColorId);
        });
        
    }
}