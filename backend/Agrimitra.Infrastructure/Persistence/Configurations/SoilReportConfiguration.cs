using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class SoilReportConfiguration : IEntityTypeConfiguration<SoilReport>
{
    public void Configure(EntityTypeBuilder<SoilReport> entity)
    {
        entity.HasKey(e => e.Id).HasName("soil_reports_pkey");

        entity.ToTable("soil_reports");

        entity.HasIndex(e => new { e.FarmerProfileId, e.ReportDate }, "ix_soil_reports_farmer")
            .IsDescending(false, true)
            .HasFilter("(NOT is_deleted)");

        entity.HasIndex(e => e.FileObjectId, "ix_soil_reports_file_object_id_98ae00");

        entity.HasIndex(e => e.InterpretationLanguage, "ix_soil_reports_interpretation_language_b20c7a");

        entity.HasIndex(e => e.LandId, "ix_soil_reports_land");

        entity.HasIndex(e => e.OcrStatus, "ix_soil_reports_ocr").HasFilter("(ocr_status = ANY (ARRAY['pending'::text, 'processing'::text, 'needs_review'::text]))");

        entity.HasIndex(e => e.SoilTestId, "ix_soil_reports_test");

        entity.HasIndex(e => e.ValidatedBy, "ix_soil_reports_validated_by_e8879e");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.FarmerProfileId).HasColumnName("farmer_profile_id");
        entity.Property(e => e.FileObjectId).HasColumnName("file_object_id");
        entity.Property(e => e.InferenceLogId).HasColumnName("inference_log_id");
        entity.Property(e => e.Interpretation).HasColumnName("interpretation");
        entity.Property(e => e.InterpretationLanguage).HasColumnName("interpretation_language");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.LandId).HasColumnName("land_id");
        entity.Property(e => e.OcrConfidence)
            .HasPrecision(6, 5)
            .HasColumnName("ocr_confidence");
        entity.Property(e => e.OcrStatus)
            .HasDefaultValueSql("'not_required'::text")
            .HasColumnName("ocr_status");
        entity.Property(e => e.ReportDate).HasColumnName("report_date");
        entity.Property(e => e.SoilTestId).HasColumnName("soil_test_id");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.ValidatedAt).HasColumnName("validated_at");
        entity.Property(e => e.ValidatedBy).HasColumnName("validated_by");

        entity.HasOne(d => d.FarmerProfile).WithMany(p => p.SoilReports)
            .HasForeignKey(d => d.FarmerProfileId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("soil_reports_farmer_profile_id_fkey");

        entity.HasOne(d => d.FileObject).WithMany(p => p.SoilReports)
            .HasForeignKey(d => d.FileObjectId)
            .HasConstraintName("soil_reports_file_object_id_fkey");

        entity.HasOne(d => d.InterpretationLanguageNavigation).WithMany(p => p.SoilReports)
            .HasForeignKey(d => d.InterpretationLanguage)
            .HasConstraintName("soil_reports_interpretation_language_fkey");

        entity.HasOne(d => d.Land).WithMany(p => p.SoilReports)
            .HasForeignKey(d => d.LandId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("soil_reports_land_id_fkey");

        entity.HasOne(d => d.SoilTest).WithMany(p => p.SoilReports)
            .HasForeignKey(d => d.SoilTestId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("soil_reports_soil_test_id_fkey");

        entity.HasOne(d => d.ValidatedByNavigation).WithMany(p => p.SoilReports)
            .HasForeignKey(d => d.ValidatedBy)
            .HasConstraintName("soil_reports_validated_by_fkey");
    }
}
