using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoreAPI.Domain;

namespace StoreAPI.Infrastructure.Configurations;

public sealed class StoreConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .ValueGeneratedOnAdd();

        builder.Property(s => s.OwnerId)
            .IsRequired();

        builder.Property(s => s.Description);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.OwnsOne(s => s.Address, address =>
        {
            address.Property(a => a.AddressLine1).HasColumnName("AddressLine1");
            address.Property(a => a.AddressLine2).HasColumnName("AddressLine2");
            address.Property(a => a.City).HasColumnName("City");
            address.Property(a => a.Province).HasColumnName("Province");
            address.Property(a => a.ZipCode).HasColumnName("Zipcode");
        });

        builder.Property(s => s.CreatedAt)
            .HasDefaultValueSql("NOW()");

        builder.Property(s => s.UpdatedAt)
            .HasDefaultValueSql("NOW()");

        builder.HasIndex(s => s.Name)
            .IsUnique();
        builder.HasIndex(s => s.OwnerId)
            .IsUnique();
    }
}