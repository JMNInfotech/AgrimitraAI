using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> entity)
    {
        entity.HasKey(e => new { e.UserId, e.RoleId }).HasName("user_roles_pkey");

        entity.ToTable("user_roles");

        entity.HasIndex(e => e.GrantedBy, "ix_user_roles_granted_by_159cd5");

        entity.HasIndex(e => e.RoleId, "ix_user_roles_role");

        entity.Property(e => e.UserId).HasColumnName("user_id");
        entity.Property(e => e.RoleId).HasColumnName("role_id");
        entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
        entity.Property(e => e.GrantedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("granted_at");
        entity.Property(e => e.GrantedBy).HasColumnName("granted_by");

        entity.HasOne(d => d.GrantedByNavigation).WithMany(p => p.UserRoleGrantedByNavigations)
            .HasForeignKey(d => d.GrantedBy)
            .HasConstraintName("user_roles_granted_by_fkey");

        entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
            .HasForeignKey(d => d.RoleId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("user_roles_role_id_fkey");

        entity.HasOne(d => d.User).WithMany(p => p.UserRoleUsers)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("user_roles_user_id_fkey");
    }
}
