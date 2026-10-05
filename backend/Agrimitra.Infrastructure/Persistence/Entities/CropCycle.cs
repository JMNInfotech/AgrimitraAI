using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class CropCycle
{
    public Guid Id { get; set; }

    public Guid LandId { get; set; }

    public Guid FarmerProfileId { get; set; }

    public Guid CropId { get; set; }

    public Guid? VarietyId { get; set; }

    public Guid? CurrentStageId { get; set; }

    public string? Name { get; set; }

    public string? Season { get; set; }

    public DateOnly PlantingDate { get; set; }

    public DateOnly? ExpectedHarvestDate { get; set; }

    public DateOnly? ActualHarvestDate { get; set; }

    public decimal? AreaValue { get; set; }

    public string? AreaUnit { get; set; }

    public int? PlantCount { get; set; }

    public Guid? IrrigationTypeId { get; set; }

    public string Status { get; set; } = null!;

    public string? QualityGrade { get; set; }

    public DateTime? ClosedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual ICollection<AiAlert> AiAlerts { get; set; } = new List<AiAlert>();

    public virtual ICollection<AiRecommendation> AiRecommendations { get; set; } = new List<AiRecommendation>();

    public virtual ICollection<ConsultantCropCarePlan> ConsultantCropCarePlans { get; set; } = new List<ConsultantCropCarePlan>();

    public virtual ICollection<ConsultationRequest> ConsultationRequests { get; set; } = new List<ConsultationRequest>();

    public virtual ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();

    public virtual Crop Crop { get; set; } = null!;

    public virtual ICollection<CropCalendarActivity> CropCalendarActivities { get; set; } = new List<CropCalendarActivity>();

    public virtual ICollection<CropCalendarPlan> CropCalendarPlans { get; set; } = new List<CropCalendarPlan>();

    public virtual CropCycleStageHistory? CropCycleStageHistory { get; set; }

    public virtual ICollection<CropDiseaseHistory> CropDiseaseHistories { get; set; } = new List<CropDiseaseHistory>();

    public virtual CropProduction? CropProduction { get; set; }

    public virtual CropStage? CurrentStage { get; set; }

    public virtual ICollection<DiseaseScan> DiseaseScans { get; set; } = new List<DiseaseScan>();

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public virtual ICollection<FarmDiary> FarmDiaries { get; set; } = new List<FarmDiary>();

    public virtual FarmerProfile FarmerProfile { get; set; } = null!;

    public virtual ICollection<HarvestRecord> HarvestRecords { get; set; } = new List<HarvestRecord>();

    public virtual ICollection<IncomeRecord> IncomeRecords { get; set; } = new List<IncomeRecord>();

    public virtual IrrigationType? IrrigationType { get; set; }

    public virtual ICollection<LabBooking> LabBookings { get; set; } = new List<LabBooking>();

    public virtual Land Land { get; set; } = null!;

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

    public virtual ICollection<ProductionRecord> ProductionRecords { get; set; } = new List<ProductionRecord>();

    public virtual ICollection<SoilSample> SoilSamples { get; set; } = new List<SoilSample>();

    public virtual CropVariety? Variety { get; set; }
}
