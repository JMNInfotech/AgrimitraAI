using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class MessageAttachment
{
    public Guid Id { get; set; }

    public Guid MessageId { get; set; }

    public Guid FileObjectId { get; set; }

    public string AttachmentKind { get; set; } = null!;

    public Guid? ThumbnailFileId { get; set; }

    public decimal? DurationSeconds { get; set; }

    public string? Waveform { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual FileObject FileObject { get; set; } = null!;

    public virtual Message Message { get; set; } = null!;

    public virtual FileObject? ThumbnailFile { get; set; }
}
