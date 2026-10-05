using Agrimitra.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agrimitra.Infrastructure.Persistence.Configurations;

internal sealed class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> b)
    {
        b.ToTable("audit_logs").HasKey(e => new { e.Id, e.CreatedAt });
        b.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
        b.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");
        b.Property(e => e.UserId).HasColumnName("user_id");
        b.Property(e => e.ActorRole).HasColumnName("actor_role");
        b.Property(e => e.Action).HasColumnName("action");
        b.Property(e => e.EntityType).HasColumnName("entity_type");
        b.Property(e => e.EntityId).HasColumnName("entity_id");
        b.Property(e => e.OldValue).HasColumnName("old_value").HasColumnType("jsonb");
        b.Property(e => e.NewValue).HasColumnName("new_value").HasColumnType("jsonb");
        b.Property(e => e.Reason).HasColumnName("reason");
        b.Property(e => e.IpAddress).HasColumnName("ip_address").HasColumnType("inet");
        b.Property(e => e.Device).HasColumnName("device");
        b.Property(e => e.CorrelationId).HasColumnName("correlation_id");
        b.HasIndex(e => new { e.UserId, e.CreatedAt }, "ix_audit_user").IsDescending(false, true);
        b.HasIndex(e => new { e.EntityType, e.EntityId, e.CreatedAt }, "ix_audit_entity").IsDescending(false, false, true);
        b.HasIndex(e => new { e.Action, e.CreatedAt }, "ix_audit_action").IsDescending(false, true);
    }
}

internal sealed class LoginAuditConfiguration : IEntityTypeConfiguration<LoginAudit>
{
    public void Configure(EntityTypeBuilder<LoginAudit> b)
    {
        b.ToTable("login_audits").HasKey(e => new { e.Id, e.CreatedAt });
        b.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
        b.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");
        b.Property(e => e.UserId).HasColumnName("user_id");
        b.Property(e => e.Identifier).HasColumnName("identifier");
        b.Property(e => e.Event).HasColumnName("event");
        b.Property(e => e.FailureReason).HasColumnName("failure_reason");
        b.Property(e => e.IpAddress).HasColumnName("ip_address").HasColumnType("inet");
        b.Property(e => e.UserAgent).HasColumnName("user_agent");
        b.Property(e => e.DeviceIdentifier).HasColumnName("device_identifier");
        b.Property(e => e.SessionId).HasColumnName("session_id");
        b.HasIndex(e => new { e.UserId, e.CreatedAt }, "ix_login_audits_user").IsDescending(false, true);
        b.HasIndex(e => new { e.IpAddress, e.CreatedAt }, "ix_login_audits_ip").IsDescending(false, true);
    }
}

internal sealed class AiInferenceLogConfiguration : IEntityTypeConfiguration<AiInferenceLog>
{
    public void Configure(EntityTypeBuilder<AiInferenceLog> b)
    {
        b.ToTable("ai_inference_logs").HasKey(e => new { e.Id, e.CreatedAt });
        b.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
        b.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");
        b.Property(e => e.ModelVersionId).HasColumnName("model_version_id");
        b.Property(e => e.Kind).HasColumnName("kind");
        b.Property(e => e.UserId).HasColumnName("user_id");
        b.Property(e => e.CropId).HasColumnName("crop_id");
        b.Property(e => e.CoarseLocation).HasColumnName("coarse_location").HasColumnType("geography(Point,4326)");
        b.Property(e => e.InputMetadata).HasColumnName("input_metadata").HasColumnType("jsonb").HasDefaultValueSql("'{}'::jsonb");
        b.Property(e => e.Output).HasColumnName("output").HasColumnType("jsonb");
        b.Property(e => e.Confidence).HasColumnName("confidence").HasPrecision(6, 5);
        b.Property(e => e.Status).HasColumnName("status").HasDefaultValueSql("'success'::text");
        b.Property(e => e.ErrorCode).HasColumnName("error_code");
        b.Property(e => e.LatencyMs).HasColumnName("latency_ms");
        b.Property(e => e.CorrelationId).HasColumnName("correlation_id");
        b.HasIndex(e => new { e.UserId, e.CreatedAt }, "ix_inference_user").IsDescending(false, true);
        b.HasIndex(e => new { e.ModelVersionId, e.CreatedAt }, "ix_inference_model").IsDescending(false, true);
        b.HasIndex(e => new { e.Kind, e.CreatedAt }, "ix_inference_kind").IsDescending(false, true);
        b.HasIndex(e => e.CropId, "ix_inference_crop");
    }
}

internal sealed class PaymentLogConfiguration : IEntityTypeConfiguration<PaymentLog>
{
    public void Configure(EntityTypeBuilder<PaymentLog> b)
    {
        b.ToTable("payment_logs").HasKey(e => new { e.Id, e.CreatedAt });
        b.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
        b.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");
        b.Property(e => e.PaymentId).HasColumnName("payment_id");
        b.Property(e => e.Event).HasColumnName("event");
        b.Property(e => e.Level).HasColumnName("level").HasDefaultValueSql("'info'::text");
        b.Property(e => e.Payload).HasColumnName("payload").HasColumnType("jsonb");
        b.Property(e => e.CorrelationId).HasColumnName("correlation_id");
        b.HasIndex(e => new { e.PaymentId, e.CreatedAt }, "ix_payment_logs_payment").IsDescending(false, true);
    }
}

internal sealed class WeatherDatumConfiguration : IEntityTypeConfiguration<WeatherDatum>
{
    public void Configure(EntityTypeBuilder<WeatherDatum> b)
    {
        b.ToTable("weather_data").HasKey(e => new { e.Id, e.ObservedOrForecastFor });
        b.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
        b.Property(e => e.WeatherLocationId).HasColumnName("weather_location_id");
        b.Property(e => e.Kind).HasColumnName("kind");
        b.Property(e => e.ObservedOrForecastFor).HasColumnName("observed_or_forecast_for");
        b.Property(e => e.FetchedAt).HasColumnName("fetched_at").HasDefaultValueSql("now()");
        b.Property(e => e.TemperatureC).HasColumnName("temperature_c").HasPrecision(5, 2);
        b.Property(e => e.TemperatureMinC).HasColumnName("temperature_min_c").HasPrecision(5, 2);
        b.Property(e => e.TemperatureMaxC).HasColumnName("temperature_max_c").HasPrecision(5, 2);
        b.Property(e => e.HumidityPercent).HasColumnName("humidity_percent").HasPrecision(5, 2);
        b.Property(e => e.RainProbabilityPercent).HasColumnName("rain_probability_percent").HasPrecision(5, 2);
        b.Property(e => e.RainfallMm).HasColumnName("rainfall_mm").HasPrecision(7, 2);
        b.Property(e => e.WindSpeedKmh).HasColumnName("wind_speed_kmh").HasPrecision(6, 2);
        b.Property(e => e.WindDirectionDeg).HasColumnName("wind_direction_deg").HasPrecision(5, 1);
        b.Property(e => e.PressureHpa).HasColumnName("pressure_hpa").HasPrecision(7, 2);
        b.Property(e => e.UvIndex).HasColumnName("uv_index").HasPrecision(4, 1);
        b.Property(e => e.ConditionCode).HasColumnName("condition_code");
        b.Property(e => e.Raw).HasColumnName("raw").HasColumnType("jsonb");
        b.HasIndex(e => new { e.WeatherLocationId, e.Kind, e.ObservedOrForecastFor }, "ux_weather_data_key").IsUnique();
    }
}

internal sealed class AdImpressionConfiguration : IEntityTypeConfiguration<AdImpression>
{
    public void Configure(EntityTypeBuilder<AdImpression> b)
    {
        b.ToTable("ad_impressions").HasKey(e => new { e.Id, e.OccurredAt });
        b.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
        b.Property(e => e.OccurredAt).HasColumnName("occurred_at").HasDefaultValueSql("now()");
        b.Property(e => e.CampaignId).HasColumnName("campaign_id");
        b.Property(e => e.CreativeId).HasColumnName("creative_id");
        b.Property(e => e.PlacementId).HasColumnName("placement_id");
        b.Property(e => e.ViewerHash).HasColumnName("viewer_hash");
        b.Property(e => e.DistrictId).HasColumnName("district_id");
        b.Property(e => e.LanguageCode).HasColumnName("language_code");
        b.Property(e => e.Cost).HasColumnName("cost").HasPrecision(12, 6).HasDefaultValue(0m);
        b.HasIndex(e => new { e.CampaignId, e.OccurredAt }, "ix_ad_impressions_campaign").IsDescending(false, true);
    }
}

internal sealed class AdClickConfiguration : IEntityTypeConfiguration<AdClick>
{
    public void Configure(EntityTypeBuilder<AdClick> b)
    {
        b.ToTable("ad_clicks").HasKey(e => new { e.Id, e.OccurredAt });
        b.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
        b.Property(e => e.OccurredAt).HasColumnName("occurred_at").HasDefaultValueSql("now()");
        b.Property(e => e.CampaignId).HasColumnName("campaign_id");
        b.Property(e => e.CreativeId).HasColumnName("creative_id");
        b.Property(e => e.PlacementId).HasColumnName("placement_id");
        b.Property(e => e.ClickToken).HasColumnName("click_token");
        b.Property(e => e.ViewerHash).HasColumnName("viewer_hash");
        b.Property(e => e.Cost).HasColumnName("cost").HasPrecision(12, 6).HasDefaultValue(0m);
        b.Property(e => e.IsValid).HasColumnName("is_valid").HasDefaultValue(true);
        b.HasIndex(e => e.ClickToken, "ix_ad_clicks_token");
    }
}

internal sealed class AdConversionConfiguration : IEntityTypeConfiguration<AdConversion>
{
    public void Configure(EntityTypeBuilder<AdConversion> b)
    {
        b.ToTable("ad_conversions").HasKey(e => new { e.Id, e.OccurredAt });
        b.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
        b.Property(e => e.OccurredAt).HasColumnName("occurred_at").HasDefaultValueSql("now()");
        b.Property(e => e.CampaignId).HasColumnName("campaign_id");
        b.Property(e => e.CreativeId).HasColumnName("creative_id");
        b.Property(e => e.ClickToken).HasColumnName("click_token");
        b.Property(e => e.ConversionType).HasColumnName("conversion_type");
        b.Property(e => e.ValueAmount).HasColumnName("value_amount").HasPrecision(14, 2);
        b.Property(e => e.ConversionRef).HasColumnName("conversion_ref");
        b.HasIndex(e => e.ClickToken, "ix_ad_conversions_token");
    }
}
