using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class PrescriptionItem
{
    public Guid Id { get; set; }

    public Guid PrescriptionId { get; set; }

    public int LineNo { get; set; }

    public string Treatment { get; set; } = null!;

    public string? ActiveIngredient { get; set; }

    public string? Dosage { get; set; }

    public decimal? DosageValue { get; set; }

    public string? DosageUnit { get; set; }

    public string? Frequency { get; set; }

    public int? DurationDays { get; set; }

    public string? ApplicationMethod { get; set; }

    public string? Precautions { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ICollection<ConsultantScheduleActivity> ConsultantScheduleActivities { get; set; } = new List<ConsultantScheduleActivity>();

    public virtual Prescription Prescription { get; set; } = null!;
}
