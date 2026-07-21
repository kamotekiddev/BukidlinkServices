using Auth.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);

        builder.Property(user => user.Id)
            .ValueGeneratedOnAdd();

        builder.Property(user => user.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(user => user.FirstName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(user => user.LastName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(user => user.HashedPassword)
            .IsRequired();

        builder.Property(user => user.VerifiedAt);

        builder.Property(user => user.CreatedAt)
            .HasDefaultValueSql("NOW()");

        builder.Property(user => user.UpdatedAt)
            .HasDefaultValueSql("NOW()");

        builder.HasMany(user => user.Roles)
            .WithMany();

        builder.HasIndex(user => user.Email)
            .IsUnique();
    }
}