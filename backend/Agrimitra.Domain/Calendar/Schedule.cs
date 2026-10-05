using Agrimitra.SharedKernel;

namespace Agrimitra.Domain.Calendar;

/// <summary>Recurrence rule. Exactly one of Count / Until may be set; both null means "until the horizon".</summary>
public sealed record RecurrenceRule(
    RecurrenceFrequency Frequency,
    int Interval,
    IReadOnlyList<DayOfWeek>? ByWeekday = null,
    int? Count = null,
    DateOnly? Until = null)
{
    public Result Validate(DateOnly start)
    {
        if (Interval < 1 || Interval > 365) return Result.Failure(CalendarErrors.InvalidRecurrence);
        if (Count is not null && Until is not null) return Result.Failure(CalendarErrors.InvalidRecurrence);
        if (Count is < 1 or > 1000) return Result.Failure(CalendarErrors.InvalidRecurrence);
        if (Until is { } u && u < start) return Result.Failure(CalendarErrors.InvalidRecurrence);
        if (ByWeekday is { Count: > 0 } && Frequency != RecurrenceFrequency.Weekly)
            return Result.Failure(CalendarErrors.InvalidRecurrence);
        return Result.Success();
    }
}

/// <summary>Local wall-clock schedule; "07:00 AM" stays 07:00 in the given IANA zone.</summary>
public sealed record Schedule(
    DateOnly StartDate,
    TimeOnly? StartTime,
    TimeOnly? EndTime,
    string TimeZoneId,
    RecurrenceRule? Recurrence = null)
{
    public Result Validate()
    {
        if (StartTime is not null && EndTime is not null && EndTime <= StartTime)
            return Result.Failure(CalendarErrors.InvalidSchedule);
        if (EndTime is not null && StartTime is null) return Result.Failure(CalendarErrors.InvalidSchedule);
        try { TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId); }
        catch (TimeZoneNotFoundException) { return Result.Failure(CalendarErrors.UnknownTimeZone); }
        return Recurrence?.Validate(StartDate) ?? Result.Success();
    }
}
