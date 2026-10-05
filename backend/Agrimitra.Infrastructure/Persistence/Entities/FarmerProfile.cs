using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class FarmerProfile
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid? OrganizationId { get; set; }

    public string FarmerCode { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public DateOnly? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public Guid? PhotoFileId { get; set; }

    public Guid? AddressId { get; set; }

    public Point? HomeLocation { get; set; }

    public string? GovIdType { get; set; }

    public byte[]? GovIdEncrypted { get; set; }

    public byte[]? GovIdBlindIndex { get; set; }

    public string? GovIdLast4 { get; set; }

    public string OnboardingState { get; set; } = null!;

    public string? FarmerSegment { get; set; }

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

    public virtual ICollection<Buyer> Buyers { get; set; } = new List<Buyer>();

    public virtual ICollection<ConsultantCropCarePlan> ConsultantCropCarePlans { get; set; } = new List<ConsultantCropCarePlan>();

    public virtual ICollection<ConsultantFarmerLink> ConsultantFarmerLinks { get; set; } = new List<ConsultantFarmerLink>();

    public virtual ICollection<ConsultantReview> ConsultantReviews { get; set; } = new List<ConsultantReview>();

    public virtual ICollection<ConsultationRequest> ConsultationRequests { get; set; } = new List<ConsultationRequest>();

    public virtual ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();

    public virtual ICollection<CropCalendarActivity> CropCalendarActivities { get; set; } = new List<CropCalendarActivity>();

    public virtual ICollection<CropCalendarPlan> CropCalendarPlans { get; set; } = new List<CropCalendarPlan>();

    public virtual ICollection<CropCycle> CropCycles { get; set; } = new List<CropCycle>();

    public virtual ICollection<DiseaseScan> DiseaseScans { get; set; } = new List<DiseaseScan>();

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public virtual ICollection<FarmDiary> FarmDiaries { get; set; } = new List<FarmDiary>();

    public virtual ICollection<Farm> Farms { get; set; } = new List<Farm>();

    public virtual ICollection<IncomeRecord> IncomeRecords { get; set; } = new List<IncomeRecord>();

    public virtual ICollection<LabBooking> LabBookings { get; set; } = new List<LabBooking>();

    public virtual ICollection<Land> Lands { get; set; } = new List<Land>();

    public virtual ICollection<Machinery> Machineries { get; set; } = new List<Machinery>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Organization? Organization { get; set; }

    public virtual FileObject? PhotoFile { get; set; }

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

    public virtual ICollection<ProductionRecord> ProductionRecords { get; set; } = new List<ProductionRecord>();

    public virtual ICollection<SoilReport> SoilReports { get; set; } = new List<SoilReport>();

    public virtual ICollection<SoilSample> SoilSamples { get; set; } = new List<SoilSample>();

    public virtual User User { get; set; } = null!;
}
