using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class CropActivityHistory
{
    public Guid Id { get; set; }

    public Guid ActivityId { get; set; }

    public Guid? OccurrenceId { get; set; }

    public string ChangeType { get; set; } = null!;

    public Guid ActorUserId { get; set; }

    public string ActorRole { get; set; } = null!;

    public string Via { get; set; } = null!;

    public string? Reason { get; set; }

    public string? BeforeValue { get; set; }

    public string? AfterValue { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual CropCalendarActivity Activity { get; set; } = null!;

    public virtual User ActorUser { get; set; } = null!;

    public virtual CropActivityOccurrence? Occurrence { get; set; }
}
