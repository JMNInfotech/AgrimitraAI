using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class Prescription
{
    public Guid Id { get; set; }

    public string PrescriptionNumber { get; set; } = null!;

    public Guid? ConsultationId { get; set; }

    public Guid ConsultantId { get; set; }

    public Guid FarmerProfileId { get; set; }

    public Guid? LandId { get; set; }

    public Guid? CropCycleId { get; set; }

    public int Version { get; set; }

    public Guid? SupersedesId { get; set; }

    public string? Diagnosis { get; set; }

    public string? Advisory { get; set; }

    public string? Precautions { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? IssuedAt { get; set; }

    public Guid? PdfFileObjectId { get; set; }

    public DateOnly? FollowUpDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual ConsultantProfile Consultant { get; set; } = null!;

    public virtual ICollection<ConsultantCropCarePlan> ConsultantCropCarePlans { get; set; } = new List<ConsultantCropCarePlan>();

    public virtual Consultation? Consultation { get; set; }

    public virtual ICollection<CropCalendarActivity> CropCalendarActivities { get; set; } = new List<CropCalendarActivity>();

    public virtual CropCycle? CropCycle { get; set; }

    public virtual FarmerProfile FarmerProfile { get; set; } = null!;

    public virtual ICollection<Prescription> InverseSupersedes { get; set; } = new List<Prescription>();

    public virtual Land? Land { get; set; }

    public virtual FileObject? PdfFileObject { get; set; }

    public virtual ICollection<PrescriptionAttachment> PrescriptionAttachments { get; set; } = new List<PrescriptionAttachment>();

    public virtual ICollection<PrescriptionFollowUp> PrescriptionFollowUps { get; set; } = new List<PrescriptionFollowUp>();

    public virtual ICollection<PrescriptionItem> PrescriptionItems { get; set; } = new List<PrescriptionItem>();

    public virtual Prescription? Supersedes { get; set; }
}
