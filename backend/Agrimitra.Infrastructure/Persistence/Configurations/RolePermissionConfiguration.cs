using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> entity)
    {
        entity.HasKey(e => new { e.RoleId, e.PermissionId }).HasName("role_permissions_pkey");

        entity.ToTable("role_permissions");

        entity.HasIndex(e => e.PermissionId, "ix_role_permissions_permission");

        entity.Property(e => e.RoleId).HasColumnName("role_id");
        entity.Property(e => e.PermissionId).HasColumnName("permission_id");
        entity.Property(e => e.GrantedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("granted_at");

        entity.HasOne(d => d.Permission).WithMany(p => p.RolePermissions)
            .HasForeignKey(d => d.PermissionId)
            .HasConstraintName("role_permissions_permission_id_fkey");

        entity.HasOne(d => d.Role).WithMany(p => p.RolePermissions)
            .HasForeignKey(d => d.RoleId)
            .HasConstraintName("role_permissions_role_id_fkey");
    }
}
