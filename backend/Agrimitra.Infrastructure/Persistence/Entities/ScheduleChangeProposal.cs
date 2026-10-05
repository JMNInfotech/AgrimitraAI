using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class ScheduleChangeProposal
{
    public Guid Id { get; set; }

    public Guid ActivityId { get; set; }

    public Guid OccurrenceId { get; set; }

    public Guid? ConflictId { get; set; }

    public string RaisedBy { get; set; } = null!;

    public Guid? RaisedByUserId { get; set; }

    public DateTime ProposedStartAt { get; set; }

    public string Rationale { get; set; } = null!;

    public Guid? ModelVersionId { get; set; }

    public Guid? DeciderUserId { get; set; }

    public string Status { get; set; } = null!;

    public Guid? DecidedBy { get; set; }

    public DateTime? DecidedAt { get; set; }

    public string? DecisionNote { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual CropCalendarActivity Activity { get; set; } = null!;

    public virtual ScheduleConflict? Conflict { get; set; }

    public virtual User? DecidedByNavigation { get; set; }

    public virtual User? DeciderUser { get; set; }

    public virtual AiModelVersion? ModelVersion { get; set; }

    public virtual CropActivityOccurrence Occurrence { get; set; } = null!;

    public virtual User? RaisedByUser { get; set; }
}
