using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class Buyer
{
    public Guid Id { get; set; }

    public Guid FarmerProfileId { get; set; }

    public string Name { get; set; } = null!;

    public string? BuyerType { get; set; }

    public string? MobileNumber { get; set; }

    public Guid? AddressId { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual Address? Address { get; set; }

    public virtual FarmerProfile FarmerProfile { get; set; } = null!;

    public virtual ICollection<IncomeRecord> IncomeRecords { get; set; } = new List<IncomeRecord>();
}
