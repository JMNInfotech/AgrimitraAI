using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class Machinery
{
    public Guid Id { get; set; }

    public Guid FarmerProfileId { get; set; }

    public string MachineryType { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Make { get; set; }

    public string? Model { get; set; }

    public string? RegistrationNumber { get; set; }

    public DateOnly? PurchaseDate { get; set; }

    public decimal? PurchasePrice { get; set; }

    public string Status { get; set; } = null!;

    public DateOnly? NextServiceDate { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual ICollection<CropCalendarActivity> CropCalendarActivities { get; set; } = new List<CropCalendarActivity>();

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public virtual FarmerProfile FarmerProfile { get; set; } = null!;

    public virtual ICollection<MachineryInsurance> MachineryInsurances { get; set; } = new List<MachineryInsurance>();

    public virtual ICollection<MachineryMaintenance> MachineryMaintenances { get; set; } = new List<MachineryMaintenance>();

    public virtual ICollection<MachineryService> MachineryServices { get; set; } = new List<MachineryService>();
}
