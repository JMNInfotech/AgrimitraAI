using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class CropCalendarPlan
{
    public Guid Id { get; set; }

    public Guid FarmerProfileId { get; set; }

    public Guid LandId { get; set; }

    public Guid CropCycleId { get; set; }

    public Guid? CarePlanId { get; set; }

    public string Name { get; set; } = null!;

    public string Source { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual ConsultantCropCarePlan? CarePlan { get; set; }

    public virtual ICollection<CropCalendarActivity> CropCalendarActivities { get; set; } = new List<CropCalendarActivity>();

    public virtual CropCycle CropCycle { get; set; } = null!;

    public virtual FarmerProfile FarmerProfile { get; set; } = null!;

    public virtual Land Land { get; set; } = null!;
}
