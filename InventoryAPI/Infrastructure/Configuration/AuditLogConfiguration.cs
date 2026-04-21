using InventoryAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryAPI.Infrastructure.Configuration;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.HasKey(auditLog => auditLog.Id);

        builder.Property(auditLog => auditLog.Id)
            .ValueGeneratedOnAdd();

        builder.Property(auditLog => auditLog.Action)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(auditLog => auditLog.EntityId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(auditLog => auditLog.EntityName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(auditLog => auditLog.Data)
            .HasColumnType("jsonb");

        builder.HasIndex(x => x.EntityId);

        builder.Property(auditLog => auditLog.CreatedAt)
            .HasDefaultValueSql("NOW()");

        builder.Property(auditLog => auditLog.UpdatedAt)
            .HasDefaultValueSql("NOW()");
    }
}