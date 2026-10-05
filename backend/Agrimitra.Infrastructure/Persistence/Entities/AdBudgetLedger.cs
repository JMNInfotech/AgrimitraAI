using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class AdBudgetLedger
{
    public Guid Id { get; set; }

    public Guid AdvertiserId { get; set; }

    public Guid? CampaignId { get; set; }

    public string EntryType { get; set; } = null!;

    public decimal Amount { get; set; }

    public decimal BalanceAfter { get; set; }

    public Guid? ReferenceId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Advertiser Advertiser { get; set; } = null!;

    public virtual AdCampaign? Campaign { get; set; }
}
