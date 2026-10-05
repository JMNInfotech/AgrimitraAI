using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class InboxMessage
{
    public string Consumer { get; set; } = null!;

    public Guid MessageId { get; set; }

    public DateTime ProcessedAt { get; set; }
}
