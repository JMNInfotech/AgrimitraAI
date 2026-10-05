using System;
using System.Collections.Generic;
using System.Net;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class RefreshToken
{
    public Guid Id { get; set; }

    public Guid SessionId { get; set; }

    public Guid UserId { get; set; }

    public string TokenHash { get; set; } = null!;

    public Guid FamilyId { get; set; }

    public Guid? ReplacedById { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public DateTime? UsedAt { get; set; }

    public DateTime? RevokedAt { get; set; }

    public IPAddress? CreatedByIp { get; set; }

    public virtual ICollection<RefreshToken> InverseReplacedBy { get; set; } = new List<RefreshToken>();

    public virtual RefreshToken? ReplacedBy { get; set; }

    public virtual UserSession Session { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
