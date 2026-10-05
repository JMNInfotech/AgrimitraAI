using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class LabResult
{
    public Guid Id { get; set; }

    public Guid SampleId { get; set; }

    public Guid TestTypeId { get; set; }

    public string Status { get; set; } = null!;

    public Guid? TechnicianId { get; set; }

    public DateTime? TechnicianSubmittedAt { get; set; }

    public Guid? ReviewerId { get; set; }

    public DateTime? ReviewedAt { get; set; }

    public string? ReviewNotes { get; set; }

    public string? Summary { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ICollection<LabResultValue> LabResultValues { get; set; } = new List<LabResultValue>();

    public virtual User? Reviewer { get; set; }

    public virtual LabSample Sample { get; set; } = null!;

    public virtual User? Technician { get; set; }

    public virtual LabTestType TestType { get; set; } = null!;
}
