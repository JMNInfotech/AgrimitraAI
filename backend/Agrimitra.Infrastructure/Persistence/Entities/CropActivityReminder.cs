using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class CropActivityReminder
{
    public Guid Id { get; set; }

    public Guid OccurrenceId { get; set; }

    public int OffsetMinutes { get; set; }

    public DateTime FireAt { get; set; }

    public List<string> Channels { get; set; } = null!;

    public string Status { get; set; } = null!;

    public int Attempts { get; set; }

    public DateTime? LockedUntil { get; set; }

    public DateTime? SentAt { get; set; }

    public string? LastError { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual CropActivityOccurrence Occurrence { get; set; } = null!;
}
