using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class AdDailyRollup
{
    public Guid CampaignId { get; set; }

    public Guid CreativeId { get; set; }

    public Guid PlacementId { get; set; }

    public DateOnly Day { get; set; }

    public long Impressions { get; set; }

    public long UniqueImpressions { get; set; }

    public long Clicks { get; set; }

    public long Conversions { get; set; }

    public decimal Spend { get; set; }

    public decimal AttributedRevenue { get; set; }

    public virtual AdCampaign Campaign { get; set; } = null!;

    public virtual AdCreative Creative { get; set; } = null!;

    public virtual AdPlacement Placement { get; set; } = null!;
}
