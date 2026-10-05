using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class Advertiser
{
    public Guid Id { get; set; }

    public Guid AdvertiserProfileId { get; set; }

    public string DisplayName { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Currency { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual ICollection<AdBilling> AdBillings { get; set; } = new List<AdBilling>();

    public virtual ICollection<AdBudgetLedger> AdBudgetLedgers { get; set; } = new List<AdBudgetLedger>();

    public virtual ICollection<AdCampaign> AdCampaigns { get; set; } = new List<AdCampaign>();

    public virtual AdvertiserProfile AdvertiserProfile { get; set; } = null!;
}
