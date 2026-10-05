using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class CropCalendarActivity
{
    public Guid Id { get; set; }

    public Guid? CalendarPlanId { get; set; }

    public Guid FarmerProfileId { get; set; }

    public Guid LandId { get; set; }

    public Guid? CropId { get; set; }

    public Guid? CropCycleId { get; set; }

    public Guid? ConsultantId { get; set; }

    public Guid? CarePlanId { get; set; }

    public int? CarePlanVersion { get; set; }

    public Guid? PrescriptionId { get; set; }

    public Guid? AiRecommendationId { get; set; }

    public Guid? MachineryId { get; set; }

    public string Origin { get; set; } = null!;

    public string ActivityType { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? Instructions { get; set; }

    public string Priority { get; set; } = null!;

    public bool IsMandatory { get; set; }

    public Guid? AssignedByUserId { get; set; }

    public DateOnly StartDate { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public string TimeZoneId { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime? CompletionDate { get; set; }

    public string? CompletionNotes { get; set; }

    public bool? IsLockedByConsultant { get; set; }

    public List<int> ReminderOffsetsMinutes { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual ActivityRecurrenceRule? ActivityRecurrenceRule { get; set; }

    public virtual AiRecommendation? AiRecommendation { get; set; }

    public virtual User? AssignedByUser { get; set; }

    public virtual CropCalendarPlan? CalendarPlan { get; set; }

    public virtual ConsultantCropCarePlan? CarePlan { get; set; }

    public virtual ConsultantProfile? Consultant { get; set; }

    public virtual ICollection<ConsultantScheduleActivity> ConsultantScheduleActivities { get; set; } = new List<ConsultantScheduleActivity>();

    public virtual Crop? Crop { get; set; }

    public virtual ICollection<CropActivityHistory> CropActivityHistories { get; set; } = new List<CropActivityHistory>();

    public virtual ICollection<CropActivityOccurrence> CropActivityOccurrences { get; set; } = new List<CropActivityOccurrence>();

    public virtual CropCycle? CropCycle { get; set; }

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public virtual ICollection<FarmDiary> FarmDiaries { get; set; } = new List<FarmDiary>();

    public virtual FarmerProfile FarmerProfile { get; set; } = null!;

    public virtual Land Land { get; set; } = null!;

    public virtual Machinery? Machinery { get; set; }

    public virtual ICollection<MachineryService> MachineryServices { get; set; } = new List<MachineryService>();

    public virtual Prescription? Prescription { get; set; }

    public virtual ICollection<ScheduleChangeProposal> ScheduleChangeProposals { get; set; } = new List<ScheduleChangeProposal>();
}
