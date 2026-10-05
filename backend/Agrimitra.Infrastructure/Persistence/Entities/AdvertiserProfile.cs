using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class AdvertiserProfile
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid? BrandProfileId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string? ContactName { get; set; }

    public string? ContactEmail { get; set; }

    public string? ContactMobile { get; set; }

    public string? Gstin { get; set; }

    public Guid? BillingAddressId { get; set; }

    public string VerificationStatus { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual Advertiser? Advertiser { get; set; }

    public virtual Address? BillingAddress { get; set; }

    public virtual BrandProfile? BrandProfile { get; set; }

    public virtual User User { get; set; } = null!;
}
