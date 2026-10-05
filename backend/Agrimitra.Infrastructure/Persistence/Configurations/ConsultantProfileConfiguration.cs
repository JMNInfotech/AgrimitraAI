using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class ConsultantProfileConfiguration : IEntityTypeConfiguration<ConsultantProfile>
{
    public void Configure(EntityTypeBuilder<ConsultantProfile> entity)
    {
        entity.HasKey(e => e.Id).HasName("consultant_profiles_pkey");

        entity.ToTable("consultant_profiles");

        entity.HasIndex(e => e.UserId, "consultant_profiles_user_id_key").IsUnique();

        entity.HasIndex(e => e.AddressId, "ix_consultant_profiles_address_id_0fae38");

        entity.HasIndex(e => e.BaseLocation, "ix_consultant_profiles_location").HasMethod("gist");

        entity.HasIndex(e => e.DisplayName, "ix_consultant_profiles_name_trgm")
            .HasMethod("gin")
            .HasOperators(new[] { "gin_trgm_ops" });

        entity.HasIndex(e => e.PhotoFileId, "ix_consultant_profiles_photo_file_id_c0d3f7");

        entity.HasIndex(e => e.VerificationStatus, "ix_consultant_profiles_verified").HasFilter("(NOT is_deleted)");

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.AddressId).HasColumnName("address_id");
        entity.Property(e => e.BaseLocation)
            .HasColumnType("geography(Point,4326)")
            .HasColumnName("base_location");
        entity.Property(e => e.Bio).HasColumnName("bio");
        entity.Property(e => e.CreatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_at");
        entity.Property(e => e.CreatedBy).HasColumnName("created_by");
        entity.Property(e => e.Currency)
            .HasMaxLength(3)
            .HasDefaultValueSql("'INR'::bpchar")
            .IsFixedLength()
            .HasColumnName("currency");
        entity.Property(e => e.DefaultFee)
            .HasPrecision(12, 2)
            .HasColumnName("default_fee");
        entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        entity.Property(e => e.DeletedBy).HasColumnName("deleted_by");
        entity.Property(e => e.DisplayName).HasColumnName("display_name");
        entity.Property(e => e.Education).HasColumnName("education");
        entity.Property(e => e.ExperienceYears).HasColumnName("experience_years");
        entity.Property(e => e.Headline).HasColumnName("headline");
        entity.Property(e => e.IsAcceptingRequests)
            .HasDefaultValue(true)
            .HasColumnName("is_accepting_requests");
        entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        entity.Property(e => e.PhotoFileId).HasColumnName("photo_file_id");
        entity.Property(e => e.RatingAverage)
            .HasPrecision(3, 2)
            .HasColumnName("rating_average");
        entity.Property(e => e.RatingCount).HasColumnName("rating_count");
        entity.Property(e => e.ServiceRadiusKm)
            .HasPrecision(8, 2)
            .HasColumnName("service_radius_km");
        entity.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("updated_at");
        entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        entity.Property(e => e.UserId).HasColumnName("user_id");
        entity.Property(e => e.VerificationStatus)
            .HasDefaultValueSql("'unverified'::text")
            .HasColumnName("verification_status");
        entity.Property(e => e.VerifiedAt).HasColumnName("verified_at");

        entity.HasOne(d => d.Address).WithMany(p => p.ConsultantProfiles)
            .HasForeignKey(d => d.AddressId)
            .HasConstraintName("consultant_profiles_address_id_fkey");

        entity.HasOne(d => d.PhotoFile).WithMany(p => p.ConsultantProfiles)
            .HasForeignKey(d => d.PhotoFileId)
            .HasConstraintName("consultant_profiles_photo_file_id_fkey");

        entity.HasOne(d => d.User).WithOne(p => p.ConsultantProfile)
            .HasForeignKey<ConsultantProfile>(d => d.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("consultant_profiles_user_id_fkey");

        entity.HasMany(d => d.LanguageCodes).WithMany(p => p.Consultants)
            .UsingEntity<Dictionary<string, object>>(
                "ConsultantLanguage",
                r => r.HasOne<Language>().WithMany()
                    .HasForeignKey("LanguageCode")
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("consultant_languages_language_code_fkey"),
                l => l.HasOne<ConsultantProfile>().WithMany()
                    .HasForeignKey("ConsultantId")
                    .HasConstraintName("consultant_languages_consultant_id_fkey"),
                j =>
                {
                    j.HasKey("ConsultantId", "LanguageCode").HasName("consultant_languages_pkey");
                    j.ToTable("consultant_languages");
                    j.HasIndex(new[] { "LanguageCode" }, "ix_consultant_languages_language_code_ee10f4");
                    j.IndexerProperty<Guid>("ConsultantId").HasColumnName("consultant_id");
                    j.IndexerProperty<string>("LanguageCode").HasColumnName("language_code");
                });
    }
}
