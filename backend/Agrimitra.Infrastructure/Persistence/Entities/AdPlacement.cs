using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class AdPlacement
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public List<string> AllowedAdTypes { get; set; } = null!;

    public int MaxItems { get; set; }

    public decimal? BasePriceCpm { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ICollection<AdCampaignPlacement> AdCampaignPlacements { get; set; } = new List<AdCampaignPlacement>();

    public virtual ICollection<AdDailyRollup> AdDailyRollups { get; set; } = new List<AdDailyRollup>();
}
