using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class Land
{
    public Guid Id { get; set; }

    public Guid FarmId { get; set; }

    public Guid FarmerProfileId { get; set; }

    public Guid? OrganizationId { get; set; }

    public string Name { get; set; } = null!;

    public string? SurveyNumber { get; set; }

    public Guid? AddressId { get; set; }

    public decimal AreaValue { get; set; }

    public string AreaUnit { get; set; } = null!;

    public decimal? AreaSqMeters { get; set; }

    public string OwnershipType { get; set; } = null!;

    public Point? Location { get; set; }

    public Guid? SoilTypeId { get; set; }

    public Guid? IrrigationTypeId { get; set; }

    public Guid? WaterSourceId { get; set; }

    public string Status { get; set; } = null!;

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual Address? Address { get; set; }

    public virtual ICollection<AiAlert> AiAlerts { get; set; } = new List<AiAlert>();

    public virtual ICollection<AiRecommendation> AiRecommendations { get; set; } = new List<AiRecommendation>();

    public virtual ICollection<ConsultantCropCarePlan> ConsultantCropCarePlans { get; set; } = new List<ConsultantCropCarePlan>();

    public virtual ICollection<ConsultationRequest> ConsultationRequests { get; set; } = new List<ConsultationRequest>();

    public virtual ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();

    public virtual ICollection<CropCalendarActivity> CropCalendarActivities { get; set; } = new List<CropCalendarActivity>();

    public virtual ICollection<CropCalendarPlan> CropCalendarPlans { get; set; } = new List<CropCalendarPlan>();

    public virtual ICollection<CropCycle> CropCycles { get; set; } = new List<CropCycle>();

    public virtual ICollection<DiseaseScan> DiseaseScans { get; set; } = new List<DiseaseScan>();

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public virtual Farm Farm { get; set; } = null!;

    public virtual ICollection<FarmDiary> FarmDiaries { get; set; } = new List<FarmDiary>();

    public virtual FarmerProfile FarmerProfile { get; set; } = null!;

    public virtual ICollection<HarvestRecord> HarvestRecords { get; set; } = new List<HarvestRecord>();

    public virtual ICollection<IncomeRecord> IncomeRecords { get; set; } = new List<IncomeRecord>();

    public virtual IrrigationType? IrrigationType { get; set; }

    public virtual ICollection<LabBooking> LabBookings { get; set; } = new List<LabBooking>();

    public virtual LandBoundary? LandBoundary { get; set; }

    public virtual ICollection<LandDocument> LandDocuments { get; set; } = new List<LandDocument>();

    public virtual ICollection<LandImage> LandImages { get; set; } = new List<LandImage>();

    public virtual LandWeatherLocation? LandWeatherLocation { get; set; }

    public virtual Organization? Organization { get; set; }

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

    public virtual ICollection<ProductionRecord> ProductionRecords { get; set; } = new List<ProductionRecord>();

    public virtual ICollection<SoilReport> SoilReports { get; set; } = new List<SoilReport>();

    public virtual ICollection<SoilSample> SoilSamples { get; set; } = new List<SoilSample>();

    public virtual SoilType? SoilType { get; set; }

    public virtual WaterSource? WaterSource { get; set; }

    public virtual ICollection<WeatherLocation> WeatherLocations { get; set; } = new List<WeatherLocation>();
}
