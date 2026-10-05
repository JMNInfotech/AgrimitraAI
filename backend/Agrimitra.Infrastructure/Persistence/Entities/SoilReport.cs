using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class SoilReport
{
    public Guid Id { get; set; }

    public Guid SoilTestId { get; set; }

    public Guid FarmerProfileId { get; set; }

    public Guid LandId { get; set; }

    public Guid? FileObjectId { get; set; }

    public string OcrStatus { get; set; } = null!;

    public decimal? OcrConfidence { get; set; }

    public Guid? ValidatedBy { get; set; }

    public DateTime? ValidatedAt { get; set; }

    public string? Interpretation { get; set; }

    public string? InterpretationLanguage { get; set; }

    public Guid? InferenceLogId { get; set; }

    public DateOnly? ReportDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual FarmerProfile FarmerProfile { get; set; } = null!;

    public virtual FileObject? FileObject { get; set; }

    public virtual Language? InterpretationLanguageNavigation { get; set; }

    public virtual ICollection<LabReport> LabReports { get; set; } = new List<LabReport>();

    public virtual Land Land { get; set; } = null!;

    public virtual ICollection<SoilMeasurement> SoilMeasurements { get; set; } = new List<SoilMeasurement>();

    public virtual SoilTest SoilTest { get; set; } = null!;

    public virtual User? ValidatedByNavigation { get; set; }
}
