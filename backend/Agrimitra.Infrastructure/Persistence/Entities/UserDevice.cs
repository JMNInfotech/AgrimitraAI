using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class UserDevice
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string DeviceIdentifier { get; set; } = null!;

    public string Platform { get; set; } = null!;

    public string? DeviceName { get; set; }

    public string? AppVersion { get; set; }

    public string? PushToken { get; set; }

    public DateTime? LastSeenAt { get; set; }

    public bool IsTrusted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<UserSession> UserSessions { get; set; } = new List<UserSession>();
}
