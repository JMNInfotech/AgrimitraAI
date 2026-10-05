using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class Expense
{
    public Guid Id { get; set; }

    public Guid FarmerProfileId { get; set; }

    public Guid CategoryId { get; set; }

    public Guid? LandId { get; set; }

    public Guid? CropId { get; set; }

    public Guid? CropCycleId { get; set; }

    public Guid? ActivityId { get; set; }

    public Guid? MachineryId { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; } = null!;

    public DateOnly ExpenseDate { get; set; }

    public string? VendorName { get; set; }

    public Guid? ReceiptFileId { get; set; }

    public Guid? OrderId { get; set; }

    public string? Notes { get; set; }

    public Guid? ClientMutationId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual CropCalendarActivity? Activity { get; set; }

    public virtual ExpenseCategory Category { get; set; } = null!;

    public virtual Crop? Crop { get; set; }

    public virtual CropCycle? CropCycle { get; set; }

    public virtual FarmerProfile FarmerProfile { get; set; } = null!;

    public virtual Land? Land { get; set; }

    public virtual Machinery? Machinery { get; set; }

    public virtual Order? Order { get; set; }

    public virtual FileObject? ReceiptFile { get; set; }
}
