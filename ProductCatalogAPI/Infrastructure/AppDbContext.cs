using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Domain;

namespace ProductCatalogAPI.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductVariant> ProductVariants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(product => product.Id);
            entity
                .Property(product => product.Id)
                .ValueGeneratedOnAdd();

            entity
                .Property(product => product.Name)
                .IsRequired()
                .HasMaxLength(255);

            entity
                .Property(product => product.Description)
                .HasMaxLength(255);
        });

        modelBuilder.Entity<ProductVariant>(entity =>
        {
            entity.HasKey(variant => variant.Id);
            entity.Property(variant => variant.Id)
                .ValueGeneratedOnAdd();

            entity.OwnsOne(variant => variant.Sku,
                builder => { builder.Property(sku => sku.Value).HasColumnName("sku").IsRequired(); });

            entity.OwnsOne(variant => variant.Price, builder =>
            {
                builder.Property(price => price.Value).HasColumnName("price").IsRequired();
                builder.Property(price => price.Currency).HasColumnName("currency").IsRequired();
            });

            entity.HasOne<Product>(variant => variant.Product)
                .WithMany(product => product.Variants)
                .HasForeignKey(variant => variant.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}