using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class ConsultantCropCarePlan
{
    public Guid Id { get; set; }

    public Guid? ConsultationId { get; set; }

    public Guid ConsultantId { get; set; }

    public Guid FarmerProfileId { get; set; }

    public Guid LandId { get; set; }

    public Guid CropId { get; set; }

    public Guid CropCycleId { get; set; }

    public Guid? PrescriptionId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string Status { get; set; } = null!;

    public int PublishedVersion { get; set; }

    public DateTime? PublishedAt { get; set; }

    public DateOnly? FollowUpDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual ConsultantProfile Consultant { get; set; } = null!;

    public virtual ICollection<ConsultantScheduleActivity> ConsultantScheduleActivities { get; set; } = new List<ConsultantScheduleActivity>();

    public virtual ICollection<ConsultantScheduleHistory> ConsultantScheduleHistories { get; set; } = new List<ConsultantScheduleHistory>();

    public virtual Consultation? Consultation { get; set; }

    public virtual Crop Crop { get; set; } = null!;

    public virtual ICollection<CropCalendarActivity> CropCalendarActivities { get; set; } = new List<CropCalendarActivity>();

    public virtual ICollection<CropCalendarPlan> CropCalendarPlans { get; set; } = new List<CropCalendarPlan>();

    public virtual CropCycle CropCycle { get; set; } = null!;

    public virtual FarmerProfile FarmerProfile { get; set; } = null!;

    public virtual Land Land { get; set; } = null!;

    public virtual Prescription? Prescription { get; set; }
}
