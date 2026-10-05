using Agrimitra.SharedKernel;

namespace Agrimitra.Domain.Calendar;

/// <summary>Append-only record of what actually happened. Never edited; corrections are new records.</summary>
public sealed record ActivityCompletion(
    Guid CompletedBy,
    DateTimeOffset CompletedAtUtc,
    DateTimeOffset ScheduledAtUtc,
    CompletionOutcome Outcome,
    string? Notes,
    string? InstructionsSnapshot,
    IReadOnlyList<string> EvidenceObjectKeys);

public sealed class ActivityOccurrence : Entity<Guid>
{
    internal ActivityOccurrence(Guid id, int seqNo, int revision, DateTimeOffset startUtc, DateTimeOffset? endUtc,
        OccurrenceStatus status) : base(id)
    {
        SeqNo = seqNo;
        Revision = revision;
        StartUtc = startUtc;
        EndUtc = endUtc;
        Status = status;
    }

    /// <summary>Position in the recurrence series; stable across horizon rollovers.</summary>
    public int SeqNo { get; }
    /// <summary>Incremented each time the occurrence is rescheduled (same SeqNo, new row).</summary>
    public int Revision { get; }
    public DateTimeOffset StartUtc { get; }
    public DateTimeOffset? EndUtc { get; }
    public OccurrenceStatus Status { get; private set; }
    public Guid? RescheduledToOccurrenceId { get; private set; }
    public ActivityCompletion? Completion { get; private set; }

    internal Result Apply(OccurrenceAction action)
    {
        if (!OccurrenceStateMachine.TryTransition(Status, action, out var next))
            return Result.Failure(CalendarErrors.InvalidTransition);
        Status = next;
        return Result.Success();
    }

    internal void LinkRescheduledTo(Guid newId) => RescheduledToOccurrenceId = newId;
    internal void Record(ActivityCompletion c) => Completion = c;

    /// <summary>"Missed" is computed, not a status.</summary>
    public bool IsMissed(DateTimeOffset now) =>
        !OccurrenceStateMachine.IsTerminal(Status) && (EndUtc ?? StartUtc) < now;
}

public sealed record ActivityHistoryEntry(
    ChangeType Type,
    Guid? OccurrenceId,
    Guid ActorUserId,
    ActorKind ActorKind,
    ChangeVia Via,
    string? Reason,
    string? Before,
    string? After,
    DateTimeOffset At);
