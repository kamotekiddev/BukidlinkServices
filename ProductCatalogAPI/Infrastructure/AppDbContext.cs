using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Domain;

namespace ProductCatalogAPI.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            entity
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(255);

            entity
                .Property(x => x.Description)
                .HasMaxLength(255);
        });
    }
}