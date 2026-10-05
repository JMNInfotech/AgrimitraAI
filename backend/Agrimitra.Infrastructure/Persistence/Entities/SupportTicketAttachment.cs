using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class SupportTicketAttachment
{
    public Guid Id { get; set; }

    public Guid TicketId { get; set; }

    public Guid? MessageId { get; set; }

    public Guid FileObjectId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual FileObject FileObject { get; set; } = null!;

    public virtual SupportTicketMessage? Message { get; set; }

    public virtual SupportTicket Ticket { get; set; } = null!;
}
