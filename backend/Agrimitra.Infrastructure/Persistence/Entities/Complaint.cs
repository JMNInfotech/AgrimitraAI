using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class Complaint
{
    public Guid Id { get; set; }

    public string ComplaintNumber { get; set; } = null!;

    public Guid ComplainantUserId { get; set; }

    public string AgainstType { get; set; } = null!;

    public Guid? AgainstId { get; set; }

    public string Category { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Status { get; set; } = null!;

    public Guid? TicketId { get; set; }

    public string? Resolution { get; set; }

    public Guid? ResolvedBy { get; set; }

    public DateTime? ResolvedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual User ComplainantUser { get; set; } = null!;

    public virtual User? ResolvedByNavigation { get; set; }

    public virtual SupportTicket? Ticket { get; set; }
}
