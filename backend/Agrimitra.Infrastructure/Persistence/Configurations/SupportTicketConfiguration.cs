using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class SupportTicketConfiguration : IEntityTypeConfiguration<SupportTicket>
{
    public void Configure(EntityTypeBuilder<SupportTicket> entity)
    {
        entity.HasKey(e => e.Id).HasName("support_tickets_pkey");

        entity.ToTable("support_tickets");

        entity.HasIndex(e => new { e.AssignedAgentId, e.Status }, "ix_tickets_agent");

        entity.HasIndex(e => new { e.RelatedEntityType, e.RelatedEntityId }, "ix_tickets_entity");

        entity.HasIndex(e => new { e.Status, e.Priority, e.CreatedAt }, "ix_tickets_queue").HasFilter("(status = ANY (ARRAY['open'::text, 'assigned'::text, 'in_progress'::text]))");

        entity.HasIndex(e => new { e.UserId, e.CreatedAt }, "ix_tickets_user").IsDescending(false, true);

        entity.HasIndex(e => e.TicketNumber, "support_tickets_ticket_number_key").IsUnique();

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.AssignedAgentId).HasColumnName("assigned_agent_id");
        entity.Property(e => e.Category).HasColumnName("category");
        entity.Property(e => e.ClosedAt).HasColumnName("closed_at");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.Description).HasColumnName("description");
        entity.Property(e => e.FirstResponseAt).HasColumnName("first_response_at");
        entity.Property(e => e.Priority)
            .HasDefaultValueSql("'normal'::text")
            .HasColumnName("priority");
        entity.Property(e => e.RelatedEntityId).HasColumnName("related_entity_id");
        entity.Property(e => e.RelatedEntityType).HasColumnName("related_entity_type");
        entity.Property(e => e.Resolution).HasColumnName("resolution");
        entity.Property(e => e.ResolvedAt).HasColumnName("resolved_at");
        entity.Property(e => e.Status)
            .HasDefaultValueSql("'open'::text")
            .HasColumnName("status");
        entity.Property(e => e.Subject).HasColumnName("subject");
        entity.Property(e => e.TicketNumber).HasColumnName("ticket_number");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.UserId).HasColumnName("user_id");

        entity.HasOne(d => d.AssignedAgent).WithMany(p => p.SupportTicketAssignedAgents)
            .HasForeignKey(d => d.AssignedAgentId)
            .HasConstraintName("support_tickets_assigned_agent_id_fkey");

        entity.HasOne(d => d.User).WithMany(p => p.SupportTicketUsers)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("support_tickets_user_id_fkey");
    }
}
