using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class BackgroundJobRunConfiguration : IEntityTypeConfiguration<BackgroundJobRun>
{
    public void Configure(EntityTypeBuilder<BackgroundJobRun> entity)
    {
        entity.HasKey(e => e.Id).HasName("background_job_runs_pkey");

        entity.ToTable("background_job_runs");

        entity.HasIndex(e => new { e.JobName, e.StartedAt }, "ix_job_runs_name").IsDescending(false, true);

        entity.Property(e => e.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasColumnName("id");
        entity.Property(e => e.Error).HasColumnName("error");
        entity.Property(e => e.FinishedAt).HasColumnName("finished_at");
        entity.Property(e => e.JobName).HasColumnName("job_name");
        entity.Property(e => e.ProcessedCount).HasColumnName("processed_count");
        entity.Property(e => e.StartedAt)
            .HasDefaultValueSql("now()")
            .HasColumnName("started_at");
        entity.Property(e => e.Status).HasColumnName("status");
    }
}
