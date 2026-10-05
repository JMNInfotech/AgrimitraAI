namespace Agrimitra.Domain.Calendar;

public static class OccurrenceStateMachine
{
    private static readonly Dictionary<OccurrenceAction, (OccurrenceStatus[] From, OccurrenceStatus To)> Table = new()
    {
        [OccurrenceAction.Confirm] = ([OccurrenceStatus.Planned], OccurrenceStatus.Scheduled),
        [OccurrenceAction.MarkUpcoming] = ([OccurrenceStatus.Scheduled], OccurrenceStatus.Upcoming),
        [OccurrenceAction.Start] = ([OccurrenceStatus.Scheduled, OccurrenceStatus.Upcoming], OccurrenceStatus.InProgress),
        [OccurrenceAction.Complete] = ([OccurrenceStatus.Scheduled, OccurrenceStatus.Upcoming, OccurrenceStatus.InProgress], OccurrenceStatus.Completed),
        [OccurrenceAction.Skip] = ([OccurrenceStatus.Scheduled, OccurrenceStatus.Upcoming, OccurrenceStatus.InProgress], OccurrenceStatus.Skipped),
        [OccurrenceAction.Cancel] = ([OccurrenceStatus.Planned, OccurrenceStatus.Scheduled, OccurrenceStatus.Upcoming, OccurrenceStatus.InProgress], OccurrenceStatus.Cancelled),
        [OccurrenceAction.Reschedule] = ([OccurrenceStatus.Planned, OccurrenceStatus.Scheduled, OccurrenceStatus.Upcoming], OccurrenceStatus.Rescheduled),
    };

    public static bool TryTransition(OccurrenceStatus from, OccurrenceAction action, out OccurrenceStatus to)
    {
        var (allowed, target) = Table[action];
        to = target;
        return allowed.Contains(from);
    }

    public static bool IsTerminal(OccurrenceStatus s) =>
        s is OccurrenceStatus.Completed or OccurrenceStatus.Skipped or OccurrenceStatus.Cancelled or OccurrenceStatus.Rescheduled;
}
