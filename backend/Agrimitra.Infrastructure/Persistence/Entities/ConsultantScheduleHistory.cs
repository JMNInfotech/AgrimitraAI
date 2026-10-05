using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class ConsultantScheduleHistory
{
    public Guid Id { get; set; }

    public Guid CarePlanId { get; set; }

    public Guid? ScheduleActivityId { get; set; }

    public string ChangeType { get; set; } = null!;

    public int? PlanVersion { get; set; }

    public Guid ActorUserId { get; set; }

    public string ActorRole { get; set; } = null!;

    public string? Reason { get; set; }

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User ActorUser { get; set; } = null!;

    public virtual ConsultantCropCarePlan CarePlan { get; set; } = null!;

    public virtual ConsultantScheduleActivity? ScheduleActivity { get; set; }
}
