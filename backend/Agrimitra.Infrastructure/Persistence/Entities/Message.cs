using System;
using System.Collections.Generic;
using NpgsqlTypes;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class Message
{
    public Guid Id { get; set; }

    public Guid RoomId { get; set; }

    public Guid SenderUserId { get; set; }

    public string MessageType { get; set; } = null!;

    public string? Body { get; set; }

    public Guid? ReplyToId { get; set; }

    public Guid? ClientMessageId { get; set; }

    public DateTime? EditedAt { get; set; }

    public NpgsqlTsVector? SearchVector { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual ICollection<Message> InverseReplyTo { get; set; } = new List<Message>();

    public virtual ICollection<MessageAttachment> MessageAttachments { get; set; } = new List<MessageAttachment>();

    public virtual ICollection<MessageReadReceipt> MessageReadReceipts { get; set; } = new List<MessageReadReceipt>();

    public virtual Message? ReplyTo { get; set; }

    public virtual ChatRoom Room { get; set; } = null!;

    public virtual User SenderUser { get; set; } = null!;
}
