using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderAPI.Models;

namespace OrderAPI.Infrastructure.Configuration;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(order => order.Id);
        builder.Property(order => order.Id)
            .ValueGeneratedOnAdd();

        builder.Property(order => order.UserId)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(order => order.UserId);

        builder.Property(order => order.PaymentMethod)
            .IsRequired();

        builder.Property(order => order.PaymentStatus)
            .IsRequired();

        builder.Property(order => order.Status)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(order => order.StoreId)
            .IsRequired();

        builder.HasIndex(order => order.StoreId);

        builder.HasMany(order => order.OrderItems)
            .WithOne()
            .HasForeignKey(orderItem => orderItem.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(order => order.Histories)
            .WithOne(history => history.Order)
            .HasForeignKey(history => history.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(order => order.CreatedAt)
            .HasDefaultValueSql("NOW()");

        builder.Property(order => order.UpdatedAt)
            .HasDefaultValueSql("NOW()");
    }
}