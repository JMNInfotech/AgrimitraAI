using Agrimitra.SharedKernel;

namespace Agrimitra.Domain.Calendar;

public sealed record ActivityScheduled(Guid ActivityId, int OccurrenceCount, DateTimeOffset OccurredAt) : IDomainEvent;
public sealed record ActivityRescheduled(Guid ActivityId, Guid OldOccurrenceId, Guid NewOccurrenceId, DateTimeOffset OccurredAt) : IDomainEvent;
public sealed record ActivityCancelled(Guid ActivityId, Guid OccurrenceId, DateTimeOffset OccurredAt) : IDomainEvent;
public sealed record ActivityCompleted(Guid ActivityId, Guid OccurrenceId, Guid CompletedBy, DateTimeOffset OccurredAt) : IDomainEvent;
public sealed record ActivitySkipped(Guid ActivityId, Guid OccurrenceId, Guid ByUser, DateTimeOffset OccurredAt) : IDomainEvent;
