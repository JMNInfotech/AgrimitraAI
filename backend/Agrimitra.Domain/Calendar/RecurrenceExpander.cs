namespace Agrimitra.Domain.Calendar;

public readonly record struct ExpandedOccurrence(int SeqNo, DateTimeOffset StartUtc, DateTimeOffset? EndUtc);

/// <summary>
/// Pure expansion of a <see cref="Schedule"/> into UTC occurrences. SeqNo counts from 0 across the whole
/// series, so re-expanding with a larger horizon yields the same SeqNo for the same occurrence
/// (needed for idempotent rolling materialisation).
/// </summary>
public static class RecurrenceExpander
{
    private const int MaxDaysScanned = 366 * 10;

    public static IReadOnlyList<ExpandedOccurrence> Expand(Schedule schedule, DateTimeOffset horizonEndUtc)
    {
        var tz = TimeZoneInfo.FindSystemTimeZoneById(schedule.TimeZoneId);
        var time = schedule.StartTime ?? TimeOnly.MinValue;
        var result = new List<ExpandedOccurrence>();
        var rule = schedule.Recurrence;

        if (rule is null)
        {
            var one = ToUtc(schedule.StartDate, time, tz);
            if (one <= horizonEndUtc) result.Add(Build(0, schedule.StartDate, schedule, tz));
            return result;
        }

        var weekdays = rule.Frequency == RecurrenceFrequency.Weekly
            ? (rule.ByWeekday is { Count: > 0 } ? rule.ByWeekday.ToHashSet() : [schedule.StartDate.DayOfWeek])
            : null;
        var weekAnchor = StartOfWeek(schedule.StartDate);
        var seq = 0;

        for (var i = 0; i < MaxDaysScanned; i++)
        {
            var day = schedule.StartDate.AddDays(i);
            if (rule.Until is { } until && day > until) break;
            if (ToUtc(day, time, tz) > horizonEndUtc) break;

            var matches = rule.Frequency switch
            {
                RecurrenceFrequency.Daily or RecurrenceFrequency.EveryNDays => i % rule.Interval == 0,
                RecurrenceFrequency.Weekly =>
                    weekdays!.Contains(day.DayOfWeek) &&
                    ((StartOfWeek(day).DayNumber - weekAnchor.DayNumber) / 7) % rule.Interval == 0,
                _ => false
            };
            if (!matches) continue;

            result.Add(Build(seq++, day, schedule, tz));
            if (rule.Count is { } count && seq >= count) break;
        }
        return result;
    }

    private static ExpandedOccurrence Build(int seq, DateOnly day, Schedule s, TimeZoneInfo tz) =>
        new(seq, ToUtc(day, s.StartTime ?? TimeOnly.MinValue, tz),
            s.EndTime is { } end ? ToUtc(day, end, tz) : null);

    private static DateOnly StartOfWeek(DateOnly d) =>
        d.AddDays(-(((int)d.DayOfWeek + 6) % 7)); // Monday-based

    private static DateTimeOffset ToUtc(DateOnly day, TimeOnly time, TimeZoneInfo tz)
    {
        var local = day.ToDateTime(time, DateTimeKind.Unspecified);
        if (tz.IsInvalidTime(local)) local = local.AddHours(1); // DST gap: move to the first valid instant
        return new DateTimeOffset(TimeZoneInfo.ConvertTimeToUtc(local, tz), TimeSpan.Zero);
    }
}
