using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class Notification
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string EventType { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Body { get; set; } = null!;

    public string? LanguageCode { get; set; }

    public string? TemplateCode { get; set; }

    public string Data { get; set; } = null!;

    public string? RelatedEntityType { get; set; }

    public Guid? RelatedEntityId { get; set; }

    public string Priority { get; set; } = null!;

    public DateTime? ReadAt { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public string? DedupeKey { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual Language? LanguageCodeNavigation { get; set; }

    public virtual ICollection<NotificationDeliveryLog> NotificationDeliveryLogs { get; set; } = new List<NotificationDeliveryLog>();

    public virtual User User { get; set; } = null!;
}
