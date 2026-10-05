using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class ActivityRecurrenceRule
{
    public Guid Id { get; set; }

    public Guid ActivityId { get; set; }

    public string Frequency { get; set; } = null!;

    public int IntervalValue { get; set; }

    public List<short>? ByWeekday { get; set; }

    public int? OccurrenceCount { get; set; }

    public DateOnly? UntilDate { get; set; }

    public DateTime? MaterializedUntil { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual CropCalendarActivity Activity { get; set; } = null!;
}
