using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class ScheduleConflict
{
    public Guid Id { get; set; }

    public Guid OccurrenceId { get; set; }

    public string Kind { get; set; } = null!;

    public string Severity { get; set; } = null!;

    public string? RuleCode { get; set; }

    public string? RuleVersion { get; set; }

    public string Evidence { get; set; } = null!;

    public Guid? WeatherDataId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime DetectedAt { get; set; }

    public DateTime? ResolvedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public Guid? WeatherAlertId { get; set; }

    public virtual CropActivityOccurrence Occurrence { get; set; } = null!;

    public virtual ICollection<ScheduleChangeProposal> ScheduleChangeProposals { get; set; } = new List<ScheduleChangeProposal>();

    public virtual WeatherAlert? WeatherAlert { get; set; }
}
