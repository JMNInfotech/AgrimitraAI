using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class AiRecommendation
{
    public Guid Id { get; set; }

    public Guid FarmerProfileId { get; set; }

    public Guid? LandId { get; set; }

    public Guid? CropCycleId { get; set; }

    public Guid? DiseaseResultId { get; set; }

    public Guid? ModelVersionId { get; set; }

    public string Kind { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string LanguageCode { get; set; } = null!;

    public string Sources { get; set; } = null!;

    public string RiskLevel { get; set; } = null!;

    public bool EscalationRequired { get; set; }

    public Guid? EscalatedConsultationId { get; set; }

    public decimal? Confidence { get; set; }

    public string Status { get; set; } = null!;

    public string Disclaimer { get; set; } = null!;

    public DateTime? ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ICollection<AiFeedback> AiFeedbacks { get; set; } = new List<AiFeedback>();

    public virtual ICollection<CropCalendarActivity> CropCalendarActivities { get; set; } = new List<CropCalendarActivity>();

    public virtual CropCycle? CropCycle { get; set; }

    public virtual DiseaseResult? DiseaseResult { get; set; }

    public virtual Consultation? EscalatedConsultation { get; set; }

    public virtual FarmerProfile FarmerProfile { get; set; } = null!;

    public virtual Land? Land { get; set; }

    public virtual Language LanguageCodeNavigation { get; set; } = null!;

    public virtual AiModelVersion? ModelVersion { get; set; }
}
