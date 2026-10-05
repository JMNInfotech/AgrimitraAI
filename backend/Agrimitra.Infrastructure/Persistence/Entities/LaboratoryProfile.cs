using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class LaboratoryProfile
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Name { get; set; } = null!;

    public string? MobileNumber { get; set; }

    public string? Email { get; set; }

    public Guid? AddressId { get; set; }

    public Point? Location { get; set; }

    public string? AccreditationNumber { get; set; }

    public Guid? ProfileImageId { get; set; }

    public string? Description { get; set; }

    public string VerificationStatus { get; set; } = null!;

    public DateTime? VerifiedAt { get; set; }

    public decimal RatingAverage { get; set; }

    public int RatingCount { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual Address? Address { get; set; }

    public virtual ICollection<LabBooking> LabBookings { get; set; } = new List<LabBooking>();

    public virtual ICollection<LabReport> LabReports { get; set; } = new List<LabReport>();

    public virtual ICollection<LabSample> LabSamples { get; set; } = new List<LabSample>();

    public virtual ICollection<LaboratoryService> LaboratoryServices { get; set; } = new List<LaboratoryService>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual FileObject? ProfileImage { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual User User { get; set; } = null!;
}
