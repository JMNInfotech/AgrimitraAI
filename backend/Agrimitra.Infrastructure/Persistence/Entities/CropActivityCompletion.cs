using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class CropActivityCompletion
{
    public Guid Id { get; set; }

    public Guid OccurrenceId { get; set; }

    public Guid CompletedBy { get; set; }

    public string Outcome { get; set; } = null!;

    public DateTime CompletedAt { get; set; }

    public DateTime ScheduledAt { get; set; }

    public string? Notes { get; set; }

    public string? InstructionsSnapshot { get; set; }

    public string Source { get; set; } = null!;

    public DateTime? ClientTimestamp { get; set; }

    public string? DeviceId { get; set; }

    public Guid? ClientMutationId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User CompletedByNavigation { get; set; } = null!;

    public virtual ICollection<CropActivityEvidence> CropActivityEvidences { get; set; } = new List<CropActivityEvidence>();

    public virtual CropActivityOccurrence Occurrence { get; set; } = null!;
}
