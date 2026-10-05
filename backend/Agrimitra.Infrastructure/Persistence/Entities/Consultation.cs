using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class Consultation
{
    public Guid Id { get; set; }

    public Guid RequestId { get; set; }

    public Guid FarmerProfileId { get; set; }

    public Guid ConsultantId { get; set; }

    public Guid ServiceId { get; set; }

    public Guid? LandId { get; set; }

    public Guid? CropCycleId { get; set; }

    public Guid? PaymentId { get; set; }

    public Guid? ChatRoomId { get; set; }

    public string Status { get; set; } = null!;

    public decimal FeeAmount { get; set; }

    public string Currency { get; set; } = null!;

    public DateTime? StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public DateTime? DataAccessExpiresAt { get; set; }

    public string? Summary { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual ICollection<AiRecommendation> AiRecommendations { get; set; } = new List<AiRecommendation>();

    public virtual ChatRoom? ChatRoom { get; set; }

    public virtual ChatRoom? ChatRoomNavigation { get; set; }

    public virtual ConsultantProfile Consultant { get; set; } = null!;

    public virtual ICollection<ConsultantCropCarePlan> ConsultantCropCarePlans { get; set; } = new List<ConsultantCropCarePlan>();

    public virtual ICollection<ConsultantFarmerLink> ConsultantFarmerLinks { get; set; } = new List<ConsultantFarmerLink>();

    public virtual ConsultantReview? ConsultantReview { get; set; }

    public virtual ICollection<ConsultationAppointment> ConsultationAppointments { get; set; } = new List<ConsultationAppointment>();

    public virtual CropCycle? CropCycle { get; set; }

    public virtual ICollection<CropDiseaseHistory> CropDiseaseHistories { get; set; } = new List<CropDiseaseHistory>();

    public virtual FarmerProfile FarmerProfile { get; set; } = null!;

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual Land? Land { get; set; }

    public virtual Payment? Payment { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

    public virtual ConsultationRequest Request { get; set; } = null!;

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ConsultationService Service { get; set; } = null!;
}
