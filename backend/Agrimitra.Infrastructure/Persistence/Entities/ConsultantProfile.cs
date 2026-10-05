using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class ConsultantProfile
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string DisplayName { get; set; } = null!;

    public Guid? PhotoFileId { get; set; }

    public string? Headline { get; set; }

    public string? Bio { get; set; }

    public string? Education { get; set; }

    public int? ExperienceYears { get; set; }

    public Guid? AddressId { get; set; }

    public Point? BaseLocation { get; set; }

    public decimal? ServiceRadiusKm { get; set; }

    public decimal? DefaultFee { get; set; }

    public string Currency { get; set; } = null!;

    public decimal RatingAverage { get; set; }

    public int RatingCount { get; set; }

    public string VerificationStatus { get; set; } = null!;

    public DateTime? VerifiedAt { get; set; }

    public bool IsAcceptingRequests { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual Address? Address { get; set; }

    public virtual ICollection<ConsultantAvailability> ConsultantAvailabilities { get; set; } = new List<ConsultantAvailability>();

    public virtual ICollection<ConsultantCropCarePlan> ConsultantCropCarePlans { get; set; } = new List<ConsultantCropCarePlan>();

    public virtual ICollection<ConsultantDocument> ConsultantDocuments { get; set; } = new List<ConsultantDocument>();

    public virtual ICollection<ConsultantExpertise> ConsultantExpertises { get; set; } = new List<ConsultantExpertise>();

    public virtual ICollection<ConsultantFarmerLink> ConsultantFarmerLinks { get; set; } = new List<ConsultantFarmerLink>();

    public virtual ICollection<ConsultantReview> ConsultantReviews { get; set; } = new List<ConsultantReview>();

    public virtual ICollection<ConsultationRequest> ConsultationRequests { get; set; } = new List<ConsultationRequest>();

    public virtual ICollection<ConsultationService> ConsultationServices { get; set; } = new List<ConsultationService>();

    public virtual ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();

    public virtual ICollection<CropCalendarActivity> CropCalendarActivities { get; set; } = new List<CropCalendarActivity>();

    public virtual FileObject? PhotoFile { get; set; }

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Language> LanguageCodes { get; set; } = new List<Language>();
}
