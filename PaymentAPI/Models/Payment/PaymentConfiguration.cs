using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PaymentAPI.Models.Payment;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(payment => payment.Id);

        builder.Property(payment => payment.ReferenceId)
            .IsRequired();

        builder.HasIndex(payment => payment.ReferenceId)
            .IsUnique();

        builder.Property(payment => payment.Type)
            .IsRequired();

        builder.HasIndex(payment => payment.Type);

        builder.Property(payment => payment.Amount)
            .IsRequired();

        builder.Property(payment => payment.CreatedAt)
            .HasDefaultValueSql("NOW()");

        builder.Property(payment => payment.UpdatedAt)
            .HasDefaultValueSql("NOW()");
    }
}