using System;
using System.Collections.Generic;
using System.Net;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class UserSession
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid? DeviceId { get; set; }

    public Guid? ActiveRoleId { get; set; }

    public IPAddress? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime LastSeenAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public DateTime? RevokedAt { get; set; }

    public string? RevokedReason { get; set; }

    public virtual Role? ActiveRole { get; set; }

    public virtual UserDevice? Device { get; set; }

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public virtual User User { get; set; } = null!;
}
