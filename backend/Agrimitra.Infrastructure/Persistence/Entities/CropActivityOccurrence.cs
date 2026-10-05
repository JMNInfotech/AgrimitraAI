using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class CropActivityOccurrence
{
    public Guid Id { get; set; }

    public Guid ActivityId { get; set; }

    public int SeqNo { get; set; }

    public int Revision { get; set; }

    public DateTime ScheduledStartAt { get; set; }

    public DateTime? ScheduledEndAt { get; set; }

    public string Status { get; set; } = null!;

    public Guid? RescheduledToId { get; set; }

    public string? OverrideInstructions { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual CropCalendarActivity Activity { get; set; } = null!;

    public virtual ICollection<AiAlert> AiAlerts { get; set; } = new List<AiAlert>();

    public virtual CropActivityCompletion? CropActivityCompletion { get; set; }

    public virtual ICollection<CropActivityEvidence> CropActivityEvidences { get; set; } = new List<CropActivityEvidence>();

    public virtual ICollection<CropActivityHistory> CropActivityHistories { get; set; } = new List<CropActivityHistory>();

    public virtual ICollection<CropActivityReminder> CropActivityReminders { get; set; } = new List<CropActivityReminder>();

    public virtual ICollection<FarmDiary> FarmDiaries { get; set; } = new List<FarmDiary>();

    public virtual ICollection<CropActivityOccurrence> InverseRescheduledTo { get; set; } = new List<CropActivityOccurrence>();

    public virtual CropActivityOccurrence? RescheduledTo { get; set; }

    public virtual ScheduleChangeProposal? ScheduleChangeProposal { get; set; }

    public virtual ICollection<ScheduleConflict> ScheduleConflicts { get; set; } = new List<ScheduleConflict>();
}
