using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class BrandProfile
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string BrandName { get; set; } = null!;

    public string? LegalName { get; set; }

    public string? Website { get; set; }

    public Guid? LogoFileId { get; set; }

    public string? Description { get; set; }

    public string VerificationStatus { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual ICollection<AdvertiserProfile> AdvertiserProfiles { get; set; } = new List<AdvertiserProfile>();

    public virtual FileObject? LogoFile { get; set; }

    public virtual User User { get; set; } = null!;
}
