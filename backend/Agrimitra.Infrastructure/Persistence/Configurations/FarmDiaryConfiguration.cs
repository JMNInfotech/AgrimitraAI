using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class FarmDiaryConfiguration : IEntityTypeConfiguration<FarmDiary>
{
    public void Configure(EntityTypeBuilder<FarmDiary> entity)
    {
        entity.HasKey(e => e.Id).HasName("farm_diary_pkey");

        entity.ToTable("farm_diary");

        entity.HasIndex(e => e.ClientMutationId, "farm_diary_client_mutation_id_key").IsUnique();

        entity.HasIndex(e => e.ActivityId, "ix_diary_activity");

        entity.HasIndex(e => e.CropId, "ix_diary_crop");

        entity.HasIndex(e => new { e.CropCycleId, e.OccurredAt }, "ix_diary_cycle").IsDescending(false, true);

        entity.HasIndex(e => new { e.FarmerProfileId, e.OccurredAt }, "ix_diary_farmer_date")
            .IsDescending(false, true)
            .HasFilter("(NOT is_deleted)");

        entity.HasIndex(e => e.LandId, "ix_diary_land");

        entity.HasIndex(e => e.OccurrenceId, "ix_diary_occurrence");

        entity.HasIndex(e => e.SearchVector, "ix_diary_search").HasMethod("gin");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.ActivityId).HasColumnName("activity_id");
        entity.Property(e => e.Body).HasColumnName("body");
        entity.Property(e => e.ClientMutationId).HasColumnName("client_mutation_id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.CropCycleId).HasColumnName("crop_cycle_id");
        entity.Property(e => e.CropId).HasColumnName("crop_id");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.EntryKind).HasColumnName("entry_kind");
        entity.Property(e => e.FarmerProfileId).HasColumnName("farmer_profile_id");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.IsPrivate)
            .HasDefaultValue(true)
            .HasColumnName("is_private");
        entity.Property(e => e.LandId).HasColumnName("land_id");
        entity.Property(e => e.OccurredAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("occurred_at");
        entity.Property(e => e.OccurrenceId).HasColumnName("occurrence_id");
        entity.Property(e => e.SearchVector)
            .HasComputedColumnSql("to_tsvector('simple'::regconfig, ((COALESCE(title, ''::text) || ' '::text) || COALESCE(body, ''::text)))", true)
            .HasColumnName("search_vector");
        entity.Property(e => e.Source)
            .HasDefaultValueSql("'manual'::text")
            .HasColumnName("source");
        entity.Property(e => e.Title).HasColumnName("title");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

        entity.HasOne(d => d.Activity).WithMany(p => p.FarmDiaries)
            .HasForeignKey(d => d.ActivityId)
            .HasConstraintName("farm_diary_activity_id_fkey");

        entity.HasOne(d => d.CropCycle).WithMany(p => p.FarmDiaries)
            .HasForeignKey(d => d.CropCycleId)
            .HasConstraintName("farm_diary_crop_cycle_id_fkey");

        entity.HasOne(d => d.Crop).WithMany(p => p.FarmDiaries)
            .HasForeignKey(d => d.CropId)
            .HasConstraintName("farm_diary_crop_id_fkey");

        entity.HasOne(d => d.FarmerProfile).WithMany(p => p.FarmDiaries)
            .HasForeignKey(d => d.FarmerProfileId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("farm_diary_farmer_profile_id_fkey");

        entity.HasOne(d => d.Land).WithMany(p => p.FarmDiaries)
            .HasForeignKey(d => d.LandId)
            .HasConstraintName("farm_diary_land_id_fkey");

        entity.HasOne(d => d.Occurrence).WithMany(p => p.FarmDiaries)
            .HasForeignKey(d => d.OccurrenceId)
            .HasConstraintName("farm_diary_occurrence_id_fkey");
    }
}
