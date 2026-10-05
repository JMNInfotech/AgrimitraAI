using System.Net;
using NetTopologySuite.Geometries;

namespace Agrimitra.Infrastructure.Persistence.Entities;

// Tables partitioned by range on the timestamp column. They are not scaffoldable, so they are mapped by hand.
// FKs are enforced by the database; navigation properties are intentionally omitted for append-only/high-volume tables.

public partial class AuditLog
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public string? ActorRole { get; set; }
    public string Action { get; set; } = null!;
    public string EntityType { get; set; } = null!;
    public string? EntityId { get; set; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public string? Reason { get; set; }
    public IPAddress? IpAddress { get; set; }
    public string? Device { get; set; }
    public string? CorrelationId { get; set; }
    public DateTime CreatedAt { get; set; }
}

public partial class LoginAudit
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public string? Identifier { get; set; }
    public string Event { get; set; } = null!;
    public string? FailureReason { get; set; }
    public IPAddress? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string? DeviceIdentifier { get; set; }
    public Guid? SessionId { get; set; }
    public DateTime CreatedAt { get; set; }
}

public partial class AiInferenceLog
{
    public Guid Id { get; set; }
    public Guid? ModelVersionId { get; set; }
    public string Kind { get; set; } = null!;
    public Guid? UserId { get; set; }
    public Guid? CropId { get; set; }
    public Point? CoarseLocation { get; set; }
    public string InputMetadata { get; set; } = "{}";
    public string? Output { get; set; }
    public decimal? Confidence { get; set; }
    public string Status { get; set; } = "success";
    public string? ErrorCode { get; set; }
    public int? LatencyMs { get; set; }
    public string? CorrelationId { get; set; }
    public DateTime CreatedAt { get; set; }
}

public partial class PaymentLog
{
    public Guid Id { get; set; }
    public Guid? PaymentId { get; set; }
    public string Event { get; set; } = null!;
    public string Level { get; set; } = "info";
    public string? Payload { get; set; }
    public string? CorrelationId { get; set; }
    public DateTime CreatedAt { get; set; }
}

public partial class WeatherDatum
{
    public Guid Id { get; set; }
    public Guid WeatherLocationId { get; set; }
    public string Kind { get; set; } = null!;
    public DateTime ObservedOrForecastFor { get; set; }
    public DateTime FetchedAt { get; set; }
    public decimal? TemperatureC { get; set; }
    public decimal? TemperatureMinC { get; set; }
    public decimal? TemperatureMaxC { get; set; }
    public decimal? HumidityPercent { get; set; }
    public decimal? RainProbabilityPercent { get; set; }
    public decimal? RainfallMm { get; set; }
    public decimal? WindSpeedKmh { get; set; }
    public decimal? WindDirectionDeg { get; set; }
    public decimal? PressureHpa { get; set; }
    public decimal? UvIndex { get; set; }
    public string? ConditionCode { get; set; }
    public string? Raw { get; set; }
}

/// <summary>Advertising events carry only a rotating pseudonymous viewer hash; never a farmer identifier.</summary>
public partial class AdImpression
{
    public Guid Id { get; set; }
    public Guid CampaignId { get; set; }
    public Guid CreativeId { get; set; }
    public Guid PlacementId { get; set; }
    public byte[] ViewerHash { get; set; } = null!;
    public Guid? DistrictId { get; set; }
    public string? LanguageCode { get; set; }
    public decimal Cost { get; set; }
    public DateTime OccurredAt { get; set; }
}

public partial class AdClick
{
    public Guid Id { get; set; }
    public Guid CampaignId { get; set; }
    public Guid CreativeId { get; set; }
    public Guid PlacementId { get; set; }
    public string ClickToken { get; set; } = null!;
    public byte[] ViewerHash { get; set; } = null!;
    public decimal Cost { get; set; }
    public bool IsValid { get; set; } = true;
    public DateTime OccurredAt { get; set; }
}

public partial class AdConversion
{
    public Guid Id { get; set; }
    public Guid CampaignId { get; set; }
    public Guid? CreativeId { get; set; }
    public string? ClickToken { get; set; }
    public string ConversionType { get; set; } = null!;
    public decimal? ValueAmount { get; set; }
    public Guid? ConversionRef { get; set; }
    public DateTime OccurredAt { get; set; }
}
