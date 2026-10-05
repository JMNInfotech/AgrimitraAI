using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class ConsultantFarmerLink
{
    public Guid Id { get; set; }

    public Guid ConsultantId { get; set; }

    public Guid FarmerProfileId { get; set; }

    public Guid? ConsultationId { get; set; }

    public string Scope { get; set; } = null!;

    public DateTime GrantedAt { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public DateTime? RevokedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual ConsultantProfile Consultant { get; set; } = null!;

    public virtual Consultation? Consultation { get; set; }

    public virtual FarmerProfile FarmerProfile { get; set; } = null!;
}
