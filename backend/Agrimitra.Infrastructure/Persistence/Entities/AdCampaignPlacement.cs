using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class AdCampaignPlacement
{
    public Guid CampaignId { get; set; }

    public Guid PlacementId { get; set; }

    public int Priority { get; set; }

    public virtual AdCampaign Campaign { get; set; } = null!;

    public virtual AdPlacement Placement { get; set; } = null!;
}
