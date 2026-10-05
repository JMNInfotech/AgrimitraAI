using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class ConsultantAvailability
{
    public Guid Id { get; set; }

    public Guid ConsultantId { get; set; }

    public short? DayOfWeek { get; set; }

    public DateOnly? SpecificDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public string TimeZoneId { get; set; } = null!;

    public bool IsUnavailable { get; set; }

    public DateOnly? ValidFrom { get; set; }

    public DateOnly? ValidUntil { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ConsultantProfile Consultant { get; set; } = null!;
}
