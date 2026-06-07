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

        builder.Property(order => order.Status)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasMany(order => order.OrderItems)
            .WithOne()
            .HasForeignKey(orderItem => orderItem.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(order => order.CreatedAt)
            .HasDefaultValueSql("NOW()");
    }
}