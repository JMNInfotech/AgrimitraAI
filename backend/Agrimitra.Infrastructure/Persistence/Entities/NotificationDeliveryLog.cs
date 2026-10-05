using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class NotificationDeliveryLog
{
    public Guid Id { get; set; }

    public Guid NotificationId { get; set; }

    public string Channel { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string? Provider { get; set; }

    public string? ProviderMessageId { get; set; }

    public int Attempt { get; set; }

    public string? ErrorCode { get; set; }

    public string? ErrorMessage { get; set; }

    public DateTime? SentAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Notification Notification { get; set; } = null!;
}
