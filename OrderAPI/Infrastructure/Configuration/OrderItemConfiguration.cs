using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderAPI.Models;

namespace OrderAPI.Infrastructure.Configuration;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(orderItem => orderItem.Id);
        builder.Property(orderItem => orderItem.Id)
            .ValueGeneratedOnAdd();

        builder.Property(orderItem => orderItem.OrderId)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(orderItem => orderItem.ProductVariantId)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(orderItem => orderItem.Quantity)
            .IsRequired()
            .HasDefaultValue(1);

        builder.Property(orderItem => orderItem.SellPrice)
            .IsRequired();

        builder.HasIndex(orderItem => orderItem.OrderId);
        builder.HasIndex(orderItem => orderItem.ProductVariantId);
    }
}