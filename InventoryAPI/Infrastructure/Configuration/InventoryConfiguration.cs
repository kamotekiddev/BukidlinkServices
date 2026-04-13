using InventoryAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryAPI.Infrastructure.Configuration;

public class InventoryConfiguration : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .ValueGeneratedOnAdd();

        builder.Property(i => i.ProductVariantId)
            .IsRequired();

        builder.Property(i => i.Quantity)
            .IsRequired();

        builder.Property(i => i.ReservedQuantity)
            .IsRequired();

        builder.HasIndex(i => i.ProductVariantId)
            .IsUnique();

        builder.Property(i => i.CreatedAt)
            .HasDefaultValueSql("NOW()");

        builder.Property(i => i.UpdatedAt)
            .HasDefaultValueSql("NOW()");
    }
}