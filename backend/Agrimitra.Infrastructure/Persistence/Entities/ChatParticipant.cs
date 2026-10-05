using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class ChatParticipant
{
    public Guid RoomId { get; set; }

    public Guid UserId { get; set; }

    public string ParticipantRole { get; set; } = null!;

    public DateTime JoinedAt { get; set; }

    public DateTime? LeftAt { get; set; }

    public bool IsMuted { get; set; }

    public DateTime? LastReadAt { get; set; }

    public virtual ChatRoom Room { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
