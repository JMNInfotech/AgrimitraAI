using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class MachineryService
{
    public Guid Id { get; set; }

    public Guid MachineryId { get; set; }

    public DateOnly ServiceDate { get; set; }

    public string? ServiceType { get; set; }

    public decimal? OdometerOrHours { get; set; }

    public decimal Cost { get; set; }

    public string? ServiceProvider { get; set; }

    public DateOnly? NextServiceDate { get; set; }

    public Guid? CalendarActivityId { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual CropCalendarActivity? CalendarActivity { get; set; }

    public virtual Machinery Machinery { get; set; } = null!;
}
