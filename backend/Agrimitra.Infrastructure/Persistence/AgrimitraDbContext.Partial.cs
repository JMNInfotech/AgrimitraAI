using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Agrimitra.Infrastructure.Persistence;

public partial class AgrimitraDbContext
{
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<LoginAudit> LoginAudits => Set<LoginAudit>();
    public DbSet<AiInferenceLog> AiInferenceLogs => Set<AiInferenceLog>();
    public DbSet<PaymentLog> PaymentLogs => Set<PaymentLog>();
    public DbSet<WeatherDatum> WeatherData => Set<WeatherDatum>();
    public DbSet<AdImpression> AdImpressions => Set<AdImpression>();
    public DbSet<AdClick> AdClicks => Set<AdClick>();
    public DbSet<AdConversion> AdConversions => Set<AdConversion>();

    /// <summary>Aggregates that use PostgreSQL's xmin system column for optimistic concurrency.</summary>
    private static readonly Type[] ConcurrencyAggregates =
    [
        typeof(Land), typeof(CropCycle), typeof(CropCalendarActivity), typeof(CropActivityOccurrence),
        typeof(ScheduleChangeProposal), typeof(ConsultantCropCarePlan), typeof(Prescription), typeof(Consultation),
        typeof(Order), typeof(Payment), typeof(AdCampaign), typeof(NurseryBatch), typeof(ProductBatch)
    ];

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        foreach (var type in ConcurrencyAggregates)
        {
            modelBuilder.Entity(type).Property<uint>("xmin")
                .HasColumnName("xmin").HasColumnType("xid").ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();
        }

        // vector(1024) embeddings
        modelBuilder.Entity<Embedding>().Property(e => e.Embedding1).HasColumnType("vector(1024)");

        // Soft delete: hide rows flagged is_deleted from every query unless IgnoreQueryFilters() is used.
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.ClrType.GetProperty(nameof(Farm.IsDeleted))?.PropertyType != typeof(bool)) continue;
            var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "e");
            var body = System.Linq.Expressions.Expression.Not(
                System.Linq.Expressions.Expression.Property(parameter, nameof(Farm.IsDeleted)));
            entityType.SetQueryFilter(System.Linq.Expressions.Expression.Lambda(body, parameter));
        }
    }
}
