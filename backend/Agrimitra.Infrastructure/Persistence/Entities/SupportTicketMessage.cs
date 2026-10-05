using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class SupportTicketMessage
{
    public Guid Id { get; set; }

    public Guid TicketId { get; set; }

    public Guid AuthorUserId { get; set; }

    public string Body { get; set; } = null!;

    public bool IsInternal { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User AuthorUser { get; set; } = null!;

    public virtual ICollection<SupportTicketAttachment> SupportTicketAttachments { get; set; } = new List<SupportTicketAttachment>();

    public virtual SupportTicket Ticket { get; set; } = null!;
}
