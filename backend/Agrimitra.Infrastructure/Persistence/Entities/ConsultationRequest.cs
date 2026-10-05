using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class ConsultationRequest
{
    public Guid Id { get; set; }

    public Guid FarmerProfileId { get; set; }

    public Guid ConsultantId { get; set; }

    public Guid ServiceId { get; set; }

    public Guid? LandId { get; set; }

    public Guid? CropCycleId { get; set; }

    public Guid? DiseaseScanId { get; set; }

    public DateTime RequestedStartAt { get; set; }

    public string? ProblemDescription { get; set; }

    public bool ShareFarmDataConsent { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? DecidedAt { get; set; }

    public string? RejectionReason { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual ConsultantProfile Consultant { get; set; } = null!;

    public virtual Consultation? Consultation { get; set; }

    public virtual CropCycle? CropCycle { get; set; }

    public virtual DiseaseScan? DiseaseScan { get; set; }

    public virtual FarmerProfile FarmerProfile { get; set; } = null!;

    public virtual Land? Land { get; set; }

    public virtual ConsultationService Service { get; set; } = null!;
}
