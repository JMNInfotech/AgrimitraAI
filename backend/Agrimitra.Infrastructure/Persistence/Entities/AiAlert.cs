using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class AiAlert
{
    public Guid Id { get; set; }

    public Guid FarmerProfileId { get; set; }

    public Guid? LandId { get; set; }

    public Guid? CropCycleId { get; set; }

    public string AlertType { get; set; } = null!;

    public string Severity { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Message { get; set; } = null!;

    public string? RuleCode { get; set; }

    public Guid? WeatherAlertId { get; set; }

    public Guid? OccurrenceId { get; set; }

    public string Status { get; set; } = null!;

    public string? DedupeKey { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual CropCycle? CropCycle { get; set; }

    public virtual FarmerProfile FarmerProfile { get; set; } = null!;

    public virtual Land? Land { get; set; }

    public virtual CropActivityOccurrence? Occurrence { get; set; }

    public virtual WeatherAlert? WeatherAlert { get; set; }
}
