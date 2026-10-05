using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class ConsultantScheduleActivity
{
    public Guid Id { get; set; }

    public Guid CarePlanId { get; set; }

    public Guid? CalendarActivityId { get; set; }

    public int PlanVersion { get; set; }

    public string ActivityType { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? Instructions { get; set; }

    public string Priority { get; set; } = null!;

    public bool IsMandatory { get; set; }

    public DateOnly StartDate { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public string TimeZoneId { get; set; } = null!;

    public string? RecurrenceFrequency { get; set; }

    public int? RecurrenceInterval { get; set; }

    public List<short>? RecurrenceWeekdays { get; set; }

    public int? RecurrenceCount { get; set; }

    public DateOnly? RecurrenceUntil { get; set; }

    public List<int> ReminderOffsetsMinutes { get; set; } = null!;

    public Guid? PrescriptionItemId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual CropCalendarActivity? CalendarActivity { get; set; }

    public virtual ConsultantCropCarePlan CarePlan { get; set; } = null!;

    public virtual ICollection<ConsultantScheduleHistory> ConsultantScheduleHistories { get; set; } = new List<ConsultantScheduleHistory>();

    public virtual PrescriptionItem? PrescriptionItem { get; set; }
}
