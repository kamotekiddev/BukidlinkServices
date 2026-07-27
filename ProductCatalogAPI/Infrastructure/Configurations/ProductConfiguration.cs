using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalogAPI.Domain;

namespace ProductCatalogAPI.Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(product => product.Id);
        builder
            .Property(product => product.Id)
            .ValueGeneratedOnAdd();

        builder
            .Property(product => product.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(product => product.StoreId)
            .IsRequired()
            .HasMaxLength(255);

        builder
            .Property(product => product.Description)
            .HasMaxLength(255);

        builder.Property(product => product.CreatedAt)
            .HasDefaultValueSql("NOW()");

        builder.Property(product => product.UpdatedAt)
            .HasDefaultValueSql("NOW()");

        builder.HasIndex(product => new { product.StoreId, product.Name })
            .IsUnique();
    }
}