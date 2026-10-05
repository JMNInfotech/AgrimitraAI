using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class WeatherAlert
{
    public Guid Id { get; set; }

    public Guid WeatherLocationId { get; set; }

    public string AlertType { get; set; } = null!;

    public string Severity { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime StartsAt { get; set; }

    public DateTime? EndsAt { get; set; }

    public string Source { get; set; } = null!;

    public string? ExternalId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ICollection<AiAlert> AiAlerts { get; set; } = new List<AiAlert>();

    public virtual ICollection<ScheduleConflict> ScheduleConflicts { get; set; } = new List<ScheduleConflict>();

    public virtual WeatherLocation WeatherLocation { get; set; } = null!;
}
