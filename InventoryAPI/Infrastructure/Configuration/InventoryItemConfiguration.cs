using InventoryAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryAPI.Infrastructure.Configuration;

public class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
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

        builder.HasIndex(i => i.ProductVariantId)
            .IsUnique();

        builder.HasMany(inventoryItem => inventoryItem.Reservations)
            .WithOne()
            .HasForeignKey(reservation => reservation.InventoryItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(i => i.CreatedAt)
            .HasDefaultValueSql("NOW()");

        builder.Property(i => i.UpdatedAt)
            .HasDefaultValueSql("NOW()");
    }
}