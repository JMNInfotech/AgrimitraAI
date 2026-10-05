using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class ConsultantReview
{
    public Guid Id { get; set; }

    public Guid ConsultationId { get; set; }

    public Guid ConsultantId { get; set; }

    public Guid FarmerProfileId { get; set; }

    public short Rating { get; set; }

    public string? Comment { get; set; }

    public string Status { get; set; } = null!;

    public Guid? ModeratedBy { get; set; }

    public DateTime? ModeratedAt { get; set; }

    public string? ConsultantReply { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual ConsultantProfile Consultant { get; set; } = null!;

    public virtual Consultation Consultation { get; set; } = null!;

    public virtual FarmerProfile FarmerProfile { get; set; } = null!;

    public virtual User? ModeratedByNavigation { get; set; }
}
