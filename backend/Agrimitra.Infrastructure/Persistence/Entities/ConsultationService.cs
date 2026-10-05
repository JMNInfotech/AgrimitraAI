using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class ConsultationService
{
    public Guid Id { get; set; }

    public Guid ConsultantId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string Mode { get; set; } = null!;

    public int DurationMinutes { get; set; }

    public decimal Fee { get; set; }

    public string Currency { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual ConsultantProfile Consultant { get; set; } = null!;

    public virtual ICollection<ConsultationRequest> ConsultationRequests { get; set; } = new List<ConsultationRequest>();

    public virtual ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();

    public virtual Product? Product { get; set; }
}
