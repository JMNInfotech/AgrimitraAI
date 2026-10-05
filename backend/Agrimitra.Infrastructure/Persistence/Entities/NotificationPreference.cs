using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class NotificationPreference
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string EventType { get; set; } = null!;

    public string Channel { get; set; } = null!;

    public bool IsEnabled { get; set; }

    public TimeOnly? QuietHoursStart { get; set; }

    public TimeOnly? QuietHoursEnd { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual User User { get; set; } = null!;
}
