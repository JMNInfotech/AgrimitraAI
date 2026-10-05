using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class AdCampaign
{
    public Guid Id { get; set; }

    public Guid AdvertiserId { get; set; }

    public string Name { get; set; } = null!;

    public string? Objective { get; set; }

    public string Status { get; set; } = null!;

    public string PricingModel { get; set; } = null!;

    public decimal? BidAmount { get; set; }

    public decimal TotalBudget { get; set; }

    public decimal? DailyBudget { get; set; }

    public decimal SpentAmount { get; set; }

    public string Currency { get; set; } = null!;

    public DateTime StartAt { get; set; }

    public DateTime EndAt { get; set; }

    public int? FrequencyCapPerDay { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public Guid? ApprovedBy { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public string? RejectionReason { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual ICollection<AdBilling> AdBillings { get; set; } = new List<AdBilling>();

    public virtual ICollection<AdBudgetLedger> AdBudgetLedgers { get; set; } = new List<AdBudgetLedger>();

    public virtual ICollection<AdCampaignPlacement> AdCampaignPlacements { get; set; } = new List<AdCampaignPlacement>();

    public virtual ICollection<AdCreative> AdCreatives { get; set; } = new List<AdCreative>();

    public virtual ICollection<AdDailyRollup> AdDailyRollups { get; set; } = new List<AdDailyRollup>();

    public virtual AdTargeting? AdTargeting { get; set; }

    public virtual Advertiser Advertiser { get; set; } = null!;

    public virtual User? ApprovedByNavigation { get; set; }
}
