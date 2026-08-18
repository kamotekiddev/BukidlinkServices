using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderAPI.Models;

namespace OrderAPI.Infrastructure.Configuration;

public class OrderHistoryConfiguration : IEntityTypeConfiguration<OrderHistory>
{
    public void Configure(EntityTypeBuilder<OrderHistory> builder)
    {
        builder.HasKey(history => history.Id);
        builder.Property(history => history.OrderId);
        builder.Property(history => history.Action)
            .IsRequired();

        builder.Property(history => history.PreviousValue)
            .HasMaxLength(255);
        builder.Property(history => history.NewValue)
            .HasMaxLength(255);

        builder.HasIndex(history => history.OrderId);

        builder.Property(order => order.CreatedAt)
            .HasDefaultValueSql("NOW()");

        builder.Property(order => order.UpdatedAt)
            .HasDefaultValueSql("NOW()");
    }
}