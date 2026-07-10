using Auth.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(role => role.Id);
        builder.Property(role => role.Id)
            .ValueGeneratedOnAdd();

        builder.Property(role => role.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(role => role.CreatedAt)
            .HasDefaultValueSql("NOW()");

        builder.Property(role => role.UpdatedAt)
            .HasDefaultValueSql("NOW()");
    }
}