using Agrimitra.SharedKernel;

namespace Agrimitra.Domain.Calendar;

public sealed class ScheduledActivity : AggregateRoot<Guid>
{
    private readonly List<ActivityOccurrence> _occurrences = [];
    private readonly List<ActivityHistoryEntry> _history = [];

    private ScheduledActivity(Guid id) : base(id) { }

    public Guid FarmerUserId { get; private init; }
    public Guid LandId { get; private init; }
    public Guid? CropCycleId { get; private init; }
    public ActivityType Type { get; private init; }
    public string Title { get; private set; } = string.Empty;
    public string? Instructions { get; private set; }
    public ActivityPriority Priority { get; private init; }
    public bool IsMandatory { get; private init; }
    public ActivityOrigin Origin { get; private init; }
    public Guid? ConsultantUserId { get; private init; }
    public Guid? CarePlanId { get; private init; }
    public Guid? PrescriptionId { get; private init; }
    public Guid? AiRecommendationId { get; private init; }
    public Schedule Schedule { get; private init; } = null!;

    /// <summary>Consultant-created activities are locked against everyone but the owning consultant.</summary>
    public bool IsLockedByConsultant => Origin == ActivityOrigin.Consultant;

    public IReadOnlyList<ActivityOccurrence> Occurrences => _occurrences;
    public IReadOnlyList<ActivityHistoryEntry> History => _history;

    public static Result<ScheduledActivity> Create(
        Guid id, Guid farmerUserId, Guid landId, Guid? cropCycleId, ActivityType type, string title,
        string? instructions, ActivityPriority priority, bool isMandatory, ActivityOrigin origin,
        Guid? consultantUserId, Guid? carePlanId, Guid? prescriptionId, Guid? aiRecommendationId,
        Schedule schedule, ActorContext creator, DateTimeOffset now)
    {
        if (string.IsNullOrWhiteSpace(title)) return Result.Failure<ScheduledActivity>(CalendarErrors.InvalidSchedule);
        if (origin == ActivityOrigin.Consultant && consultantUserId is null)
            return Result.Failure<ScheduledActivity>(CalendarErrors.InvalidSchedule);
        if (origin == ActivityOrigin.Consultant && !(creator.Kind == ActorKind.Consultant && creator.UserId == consultantUserId))
            return Result.Failure<ScheduledActivity>(CalendarErrors.Forbidden);
        if (origin != ActivityOrigin.Consultant && creator.Kind == ActorKind.AiService)
            return Result.Failure<ScheduledActivity>(CalendarErrors.Forbidden);
        var valid = schedule.Validate();
        if (valid.IsFailure) return Result.Failure<ScheduledActivity>(valid.Error);

        var a = new ScheduledActivity(id)
        {
            FarmerUserId = farmerUserId, LandId = landId, CropCycleId = cropCycleId, Type = type,
            Title = title.Trim(), Instructions = instructions, Priority = priority, IsMandatory = isMandatory,
            Origin = origin, ConsultantUserId = consultantUserId, CarePlanId = carePlanId,
            PrescriptionId = prescriptionId, AiRecommendationId = aiRecommendationId, Schedule = schedule
        };
        a.AddHistory(ChangeType.Created, null, creator, ChangeVia.Direct, null, null, title, now);
        return Result.Success(a);
    }

    /// <summary>Idempotently adds occurrences up to the horizon (rolling materialisation).</summary>
    public int MaterializeOccurrences(DateTimeOffset horizonEndUtc, DateTimeOffset now)
    {
        var existing = _occurrences.Select(o => o.SeqNo).ToHashSet();
        var added = 0;
        foreach (var e in RecurrenceExpander.Expand(Schedule, horizonEndUtc))
        {
            if (existing.Contains(e.SeqNo)) continue;
            _occurrences.Add(new ActivityOccurrence(Guid.NewGuid(), e.SeqNo, 0, e.StartUtc, e.EndUtc, OccurrenceStatus.Scheduled));
            added++;
        }
        if (added > 0) Raise(new ActivityScheduled(Id, added, now));
        return added;
    }

    public Result Reschedule(Guid occurrenceId, DateTimeOffset newStartUtc, DateTimeOffset? newEndUtc, ActorContext actor,
        ChangeVia via, string? reason, DateTimeOffset now)
    {
        var auth = AuthorizeScheduleChange(actor, via);
        if (auth.IsFailure) return auth;
        if (newStartUtc <= now) return Result.Failure(CalendarErrors.PastTime);
        if (newEndUtc is { } end && end <= newStartUtc) return Result.Failure(CalendarErrors.InvalidSchedule);

        var occ = Find(occurrenceId);
        if (occ is null) return Result.Failure(CalendarErrors.OccurrenceNotFound);
        var before = occ.StartUtc;
        var moved = occ.Apply(OccurrenceAction.Reschedule);
        if (moved.IsFailure) return moved;

        var fresh = new ActivityOccurrence(Guid.NewGuid(), occ.SeqNo, occ.Revision + 1, newStartUtc, newEndUtc, OccurrenceStatus.Scheduled);
        occ.LinkRescheduledTo(fresh.Id);
        _occurrences.Add(fresh);
        AddHistory(ChangeType.Rescheduled, occ.Id, actor, via, reason, before.ToString("O"), newStartUtc.ToString("O"), now);
        Raise(new ActivityRescheduled(Id, occ.Id, fresh.Id, now));
        return Result.Success();
    }

    public Result Cancel(Guid occurrenceId, ActorContext actor, string? reason, DateTimeOffset now)
    {
        var auth = AuthorizeScheduleChange(actor, ChangeVia.Direct);
        if (auth.IsFailure) return auth;
        var occ = Find(occurrenceId);
        if (occ is null) return Result.Failure(CalendarErrors.OccurrenceNotFound);
        var r = occ.Apply(OccurrenceAction.Cancel);
        if (r.IsFailure) return r;
        AddHistory(ChangeType.Cancelled, occ.Id, actor, ChangeVia.Direct, reason, null, null, now);
        Raise(new ActivityCancelled(Id, occ.Id, now));
        return Result.Success();
    }

    public Result Complete(Guid occurrenceId, ActorContext actor, string? notes, IReadOnlyList<string>? evidenceKeys,
        DateTimeOffset completedAtUtc, DateTimeOffset now)
    {
        if (!ScheduleAuthorizationPolicy.CanComplete(this, actor)) return Result.Failure(CalendarErrors.Forbidden);
        var occ = Find(occurrenceId);
        if (occ is null) return Result.Failure(CalendarErrors.OccurrenceNotFound);
        var r = occ.Apply(OccurrenceAction.Complete);
        if (r.IsFailure) return r;
        occ.Record(new ActivityCompletion(actor.UserId, completedAtUtc, occ.StartUtc, CompletionOutcome.Completed,
            notes, Instructions, evidenceKeys ?? []));
        AddHistory(ChangeType.Completed, occ.Id, actor, ChangeVia.Direct, notes, null, null, now);
        Raise(new ActivityCompleted(Id, occ.Id, actor.UserId, now));
        return Result.Success();
    }

    /// <summary>
    /// Farmer skip / not-applicable. A mandatory consultant activity cannot be skipped directly:
    /// <see cref="CalendarErrors.ApprovalRequired"/> tells the application layer to open a change request.
    /// </summary>
    public Result Skip(Guid occurrenceId, ActorContext actor, string reason, bool notApplicable, DateTimeOffset now)
    {
        if (string.IsNullOrWhiteSpace(reason)) return Result.Failure(CalendarErrors.ReasonRequired);
        if (!ScheduleAuthorizationPolicy.CanComplete(this, actor)) return Result.Failure(CalendarErrors.Forbidden);
        if (IsLockedByConsultant && IsMandatory) return Result.Failure(CalendarErrors.ApprovalRequired);
        var occ = Find(occurrenceId);
        if (occ is null) return Result.Failure(CalendarErrors.OccurrenceNotFound);
        var r = occ.Apply(OccurrenceAction.Skip);
        if (r.IsFailure) return r;
        occ.Record(new ActivityCompletion(actor.UserId, now, occ.StartUtc,
            notApplicable ? CompletionOutcome.NotApplicable : CompletionOutcome.Skipped, reason, Instructions, []));
        AddHistory(ChangeType.Skipped, occ.Id, actor, ChangeVia.Direct, reason, null, null, now);
        Raise(new ActivitySkipped(Id, occ.Id, actor.UserId, now));
        return Result.Success();
    }

    /// <summary>Worker-driven: Scheduled → Upcoming for occurrences starting within the window.</summary>
    public int MarkUpcoming(DateTimeOffset now, TimeSpan window)
    {
        var n = 0;
        foreach (var o in _occurrences.Where(o => o.Status == OccurrenceStatus.Scheduled && o.StartUtc - now <= window && o.StartUtc > now))
            if (o.Apply(OccurrenceAction.MarkUpcoming).IsSuccess) n++;
        return n;
    }

    private Result AuthorizeScheduleChange(ActorContext actor, ChangeVia via)
    {
        if (via is ChangeVia.AiProposalApproval or ChangeVia.FarmerRequestApproval)
            return ScheduleAuthorizationPolicy.CanDecideProposal(this, actor) ||
                   (actor.Kind == ActorKind.Admin && ScheduleAuthorizationPolicy.CanChangeSchedule(this, actor))
                ? Result.Success()
                : Result.Failure(CalendarErrors.Forbidden);
        return ScheduleAuthorizationPolicy.CanChangeSchedule(this, actor) ? Result.Success() : Result.Failure(CalendarErrors.Forbidden);
    }

    private ActivityOccurrence? Find(Guid id) => _occurrences.FirstOrDefault(o => o.Id == id);

    private void AddHistory(ChangeType t, Guid? occ, ActorContext actor, ChangeVia via, string? reason, string? before,
        string? after, DateTimeOffset at) =>
        _history.Add(new ActivityHistoryEntry(t, occ, actor.UserId, actor.Kind, via, reason, before, after, at));

    internal void AddProposalHistory(ChangeType t, Guid occ, ActorContext actor, ChangeVia via, string? reason, DateTimeOffset at) =>
        AddHistory(t, occ, actor, via, reason, null, null, at);
}
