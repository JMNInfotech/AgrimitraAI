using System;
using System.Collections.Generic;
using NpgsqlTypes;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class FarmDiary
{
    public Guid Id { get; set; }

    public Guid FarmerProfileId { get; set; }

    public Guid? LandId { get; set; }

    public Guid? CropId { get; set; }

    public Guid? CropCycleId { get; set; }

    public Guid? ActivityId { get; set; }

    public Guid? OccurrenceId { get; set; }

    public string EntryKind { get; set; } = null!;

    public string? Title { get; set; }

    public string? Body { get; set; }

    public DateTime OccurredAt { get; set; }

    public string Source { get; set; } = null!;

    public bool IsPrivate { get; set; }

    public Guid? ClientMutationId { get; set; }

    public NpgsqlTsVector? SearchVector { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public virtual CropCalendarActivity? Activity { get; set; }

    public virtual Crop? Crop { get; set; }

    public virtual CropCycle? CropCycle { get; set; }

    public virtual ICollection<FarmDiaryAttachment> FarmDiaryAttachments { get; set; } = new List<FarmDiaryAttachment>();

    public virtual FarmerProfile FarmerProfile { get; set; } = null!;

    public virtual Land? Land { get; set; }

    public virtual CropActivityOccurrence? Occurrence { get; set; }
}
