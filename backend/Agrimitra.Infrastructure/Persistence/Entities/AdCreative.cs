using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class AdCreative
{
    public Guid Id { get; set; }

    public Guid CampaignId { get; set; }

    public string AdType { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public Guid? ProductId { get; set; }

    public Guid? CategoryId { get; set; }

    public Guid? ImageFileId { get; set; }

    public Guid? VideoFileId { get; set; }

    public string? CtaLabel { get; set; }

    public string? LandingUrl { get; set; }

    public string? LanguageCode { get; set; }

    public string SponsoredLabel { get; set; } = null!;

    public string Status { get; set; } = null!;

    public Guid? ReviewedBy { get; set; }

    public DateTime? ReviewedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual ICollection<AdDailyRollup> AdDailyRollups { get; set; } = new List<AdDailyRollup>();

    public virtual AdCampaign Campaign { get; set; } = null!;

    public virtual ProductCategory? Category { get; set; }

    public virtual FileObject? ImageFile { get; set; }

    public virtual Language? LanguageCodeNavigation { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? ReviewedByNavigation { get; set; }

    public virtual FileObject? VideoFile { get; set; }
}
