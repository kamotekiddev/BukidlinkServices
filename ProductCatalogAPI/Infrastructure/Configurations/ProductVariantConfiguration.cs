using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalogAPI.Domain;

namespace ProductCatalogAPI.Infrastructure.Configurations;

public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.HasKey(variant => variant.Id);

        builder.Property(variant => variant.Id)
            .ValueGeneratedOnAdd();

        builder.Property(variant => variant.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(variant => variant.CreatedAt)
            .HasDefaultValueSql("NOW()");

        builder.Property(variant => variant.UpdatedAt)
            .HasDefaultValueSql("NOW()");

        builder.OwnsOne(variant => variant.Sku,
            navigationBuilder => { navigationBuilder.Property(sku => sku.Value).HasColumnName("Sku").IsRequired(); });

        builder.OwnsOne(variant => variant.Price, navigationBuilder =>
        {
            navigationBuilder.Property(price => price.Value).HasColumnName("Price").IsRequired();
            navigationBuilder.Property(price => price.Currency).HasColumnName("Currency").IsRequired();
        });

        builder.HasOne<Product>(variant => variant.Product)
            .WithMany(product => product.Variants)
            .HasForeignKey(variant => variant.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(variant => new { variant.ProductId, variant.Name })
            .IsUnique();
    }
}