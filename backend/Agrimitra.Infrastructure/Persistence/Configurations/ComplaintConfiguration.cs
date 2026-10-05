using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class ComplaintConfiguration : IEntityTypeConfiguration<Complaint>
{
    public void Configure(EntityTypeBuilder<Complaint> entity)
    {
        entity.HasKey(e => e.Id).HasName("complaints_pkey");

        entity.ToTable("complaints");

        entity.HasIndex(e => e.ComplaintNumber, "complaints_complaint_number_key").IsUnique();

        entity.HasIndex(e => new { e.AgainstType, e.AgainstId }, "ix_complaints_against");

        entity.HasIndex(e => e.ComplainantUserId, "ix_complaints_complainant");

        entity.HasIndex(e => e.ResolvedBy, "ix_complaints_resolved_by_01c584");

        entity.HasIndex(e => new { e.Status, e.CreatedAt }, "ix_complaints_status").HasFilter("(status = ANY (ARRAY['open'::text, 'under_review'::text, 'escalated'::text]))");

        entity.HasIndex(e => e.TicketId, "ix_complaints_ticket");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.AgainstId).HasColumnName("against_id");
        entity.Property(e => e.AgainstType).HasColumnName("against_type");
        entity.Property(e => e.Category).HasColumnName("category");
        entity.Property(e => e.ComplainantUserId).HasColumnName("complainant_user_id");
        entity.Property(e => e.ComplaintNumber).HasColumnName("complaint_number");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.Description).HasColumnName("description");
        entity.Property(e => e.Resolution).HasColumnName("resolution");
        entity.Property(e => e.ResolvedAt).HasColumnName("resolved_at");
        entity.Property(e => e.ResolvedBy).HasColumnName("resolved_by");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'open'::text")
            .HasColumnName("status");
        entity.Property(e => e.TicketId).HasColumnName("ticket_id");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.ComplainantUser).WithMany(p => p.ComplaintComplainantUsers)
            .HasForeignKey(d => d.ComplainantUserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("complaints_complainant_user_id_fkey");

        entity.HasOne(d => d.ResolvedByNavigation).WithMany(p => p.ComplaintResolvedByNavigations)
            .HasForeignKey(d => d.ResolvedBy)
            .HasConstraintName("complaints_resolved_by_fkey");

        entity.HasOne(d => d.Ticket).WithMany(p => p.Complaints)
            .HasForeignKey(d => d.TicketId)
            .HasConstraintName("complaints_ticket_id_fkey");
    }
}
