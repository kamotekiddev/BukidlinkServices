using InventoryAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryAPI.Infrastructure.Configuration;

public class InventoryReservationConfiguration : IEntityTypeConfiguration<InventoryReservation>
{
    public void Configure(EntityTypeBuilder<InventoryReservation> builder)
    {
        builder.HasKey(reservation => reservation.Id);
        builder.Property(reservation => reservation.Id)
            .ValueGeneratedOnAdd();

        builder.Property(reservation => reservation.InventoryItemId)
            .IsRequired();

        builder.HasIndex(reservation => reservation.InventoryItemId)
            .IsUnique();

        builder.Property(reservation => reservation.Quantity)
            .IsRequired();

        builder.Property(reservation => reservation.CreatedAt)
            .HasDefaultValueSql("NOW()");

        builder.Property(reservation => reservation.UpdatedAt)
            .HasDefaultValueSql("NOW()");
    }
}