using Agrimitra.SharedKernel;

namespace Agrimitra.Domain.Calendar;

public enum ProposalStatus { Pending, Approved, Rejected, Expired }
public enum ProposalRaisedBy { Ai, System, Farmer }

/// <summary>
/// A suggestion to move an occurrence (weather conflict, farmer request, ...). It never changes the schedule by itself:
/// approval by the authorised decider is the only path, and it runs through the activity's normal reschedule rules.
/// </summary>
public sealed class ScheduleChangeProposal : AggregateRoot<Guid>
{
    private ScheduleChangeProposal(Guid id) : base(id) { }

    public Guid ActivityId { get; private init; }
    public Guid OccurrenceId { get; private init; }
    public DateTimeOffset ProposedStartUtc { get; private init; }
    public string Rationale { get; private init; } = string.Empty;
    public ProposalRaisedBy RaisedBy { get; private init; }
    public ProposalStatus Status { get; private set; } = ProposalStatus.Pending;
    public Guid? DecidedBy { get; private set; }
    public string? DecisionNote { get; private set; }

    public static ScheduleChangeProposal Create(Guid id, Guid activityId, Guid occurrenceId, DateTimeOffset proposedStartUtc,
        string rationale, ProposalRaisedBy raisedBy) =>
        new(id) { ActivityId = activityId, OccurrenceId = occurrenceId, ProposedStartUtc = proposedStartUtc, Rationale = rationale, RaisedBy = raisedBy };

    public Result Approve(ScheduledActivity activity, ActorContext decider, string? note, DateTimeOffset now)
    {
        if (Status != ProposalStatus.Pending) return Result.Failure(CalendarErrors.ProposalNotPending);
        var via = RaisedBy == ProposalRaisedBy.Farmer ? ChangeVia.FarmerRequestApproval : ChangeVia.AiProposalApproval;
        var r = activity.Reschedule(OccurrenceId, ProposedStartUtc, null, decider, via, note ?? Rationale, now);
        if (r.IsFailure) return r;
        Status = ProposalStatus.Approved;
        DecidedBy = decider.UserId;
        DecisionNote = note;
        activity.AddProposalHistory(ChangeType.ProposalApproved, OccurrenceId, decider, via, note, now);
        return Result.Success();
    }

    public Result Reject(ScheduledActivity activity, ActorContext decider, string? note, DateTimeOffset now)
    {
        if (Status != ProposalStatus.Pending) return Result.Failure(CalendarErrors.ProposalNotPending);
        if (!ScheduleAuthorizationPolicy.CanDecideProposal(activity, decider)) return Result.Failure(CalendarErrors.Forbidden);
        Status = ProposalStatus.Rejected;
        DecidedBy = decider.UserId;
        DecisionNote = note;
        activity.AddProposalHistory(ChangeType.ProposalRejected, OccurrenceId, decider,
            RaisedBy == ProposalRaisedBy.Farmer ? ChangeVia.FarmerRequestApproval : ChangeVia.AiProposalApproval, note, now);
        return Result.Success();
    }

    public void Expire() { if (Status == ProposalStatus.Pending) Status = ProposalStatus.Expired; }
}
