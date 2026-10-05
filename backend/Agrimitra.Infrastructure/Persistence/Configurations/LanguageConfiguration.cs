using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class LanguageConfiguration : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> entity)
    {
        entity.HasKey(e => e.Code).HasName("languages_pkey");

        entity.ToTable("languages");

        entity.Property(e => e.Code).HasColumnName("code");
        entity.Property(e => e.IsActive)
            .HasDefaultValue(true)
            .HasColumnName("is_active");
        entity.Property(e => e.IsRtl).HasColumnName("is_rtl");
        entity.Property(e => e.Name).HasColumnName("name");
        entity.Property(e => e.NativeName).HasColumnName("native_name");
        entity.Property(e => e.SortOrder).HasColumnName("sort_order");
    }
}
