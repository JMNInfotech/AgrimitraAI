using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class VerificationDocument
{
    public Guid Id { get; set; }

    public Guid VerificationRecordId { get; set; }

    public string DocumentType { get; set; } = null!;

    public byte[]? DocumentNumberEncrypted { get; set; }

    public Guid FileObjectId { get; set; }

    public DateOnly? ExpiresOn { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual FileObject FileObject { get; set; } = null!;

    public virtual VerificationRecord VerificationRecord { get; set; } = null!;
}
