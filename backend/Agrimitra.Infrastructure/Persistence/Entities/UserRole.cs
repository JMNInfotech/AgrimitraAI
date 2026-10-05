using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class UserRole
{
    public Guid UserId { get; set; }

    public Guid RoleId { get; set; }

    public Guid? GrantedBy { get; set; }

    public DateTime GrantedAt { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public virtual User? GrantedByNavigation { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
