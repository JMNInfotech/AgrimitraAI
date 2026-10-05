using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class Review
{
    public Guid Id { get; set; }

    public Guid ReviewerUserId { get; set; }

    public string TargetType { get; set; } = null!;

    public Guid? ProductId { get; set; }

    public Guid? NurseryId { get; set; }

    public Guid? LaboratoryId { get; set; }

    public Guid? ShopId { get; set; }

    public Guid? ConsultantId { get; set; }

    public Guid? OrderItemId { get; set; }

    public Guid? LabBookingId { get; set; }

    public Guid? ConsultationId { get; set; }

    public short Rating { get; set; }

    public string? Title { get; set; }

    public string? Comment { get; set; }

    public string Status { get; set; } = null!;

    public Guid? ModeratedBy { get; set; }

    public DateTime? ModeratedAt { get; set; }

    public string? ModerationReason { get; set; }

    public string? SellerReply { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual ConsultantProfile? Consultant { get; set; }

    public virtual Consultation? Consultation { get; set; }

    public virtual LabBooking? LabBooking { get; set; }

    public virtual LaboratoryProfile? Laboratory { get; set; }

    public virtual User? ModeratedByNavigation { get; set; }

    public virtual NurseryProfile? Nursery { get; set; }

    public virtual OrderItem? OrderItem { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User ReviewerUser { get; set; } = null!;

    public virtual ShopProfile? Shop { get; set; }
}
