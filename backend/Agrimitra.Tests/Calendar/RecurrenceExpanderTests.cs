using Agrimitra.Domain.Calendar;

namespace Agrimitra.Tests.Calendar;

public class RecurrenceExpanderTests
{
    private static readonly DateTimeOffset Horizon = new(2027, 1, 1, 0, 0, 0, TimeSpan.Zero);

    [Fact]
    public void OneTime_Is_Converted_From_Local_To_Utc()
    {
        var o = RecurrenceExpander.Expand(Fx.OneTime(), Horizon).Single();
        Assert.Equal(new DateTimeOffset(2026, 10, 15, 1, 30, 0, TimeSpan.Zero), o.StartUtc); // 07:00 IST
    }

    [Fact]
    public void EveryNDays_With_Count_Stops_At_Count()
    {
        var r = RecurrenceExpander.Expand(Fx.Recurring(new(RecurrenceFrequency.EveryNDays, 7, Count: 4)), Horizon);
        Assert.Equal(4, r.Count);
        Assert.All(r.Zip(r.Skip(1)), p => Assert.Equal(TimeSpan.FromDays(7), p.Second.StartUtc - p.First.StartUtc));
        Assert.Equal([0, 1, 2, 3], r.Select(x => x.SeqNo));
    }

    [Fact]
    public void Daily_With_Until_Is_Inclusive()
    {
        var r = RecurrenceExpander.Expand(
            Fx.Recurring(new(RecurrenceFrequency.Daily, 1, Until: new DateOnly(2026, 10, 19))), Horizon);
        Assert.Equal(5, r.Count);
    }

    [Fact]
    public void Weekly_ByWeekday_Only_Yields_Selected_Days()
    {
        var days = new[] { DayOfWeek.Monday, DayOfWeek.Thursday };
        var r = RecurrenceExpander.Expand(Fx.Recurring(new(RecurrenceFrequency.Weekly, 1, days, Count: 6)), Horizon);
        Assert.Equal(6, r.Count);
        var tz = TimeZoneInfo.FindSystemTimeZoneById(Fx.Ist);
        Assert.All(r, o => Assert.Contains(TimeZoneInfo.ConvertTime(o.StartUtc, tz).DayOfWeek, days));
    }

    [Fact]
    public void Weekly_Interval_2_Skips_Alternate_Weeks()
    {
        var r = RecurrenceExpander.Expand(Fx.Recurring(new(RecurrenceFrequency.Weekly, 2, Count: 3)), Horizon);
        Assert.Equal(TimeSpan.FromDays(14), r[1].StartUtc - r[0].StartUtc);
    }

    [Fact]
    public void Expansion_Is_Stable_When_Horizon_Grows()
    {
        var rule = Fx.Recurring(new(RecurrenceFrequency.Daily, 1));
        var small = RecurrenceExpander.Expand(rule, new DateTimeOffset(2026, 11, 1, 0, 0, 0, TimeSpan.Zero));
        var big = RecurrenceExpander.Expand(rule, new DateTimeOffset(2026, 12, 1, 0, 0, 0, TimeSpan.Zero));
        Assert.True(big.Count > small.Count);
        Assert.Equal(small, big.Take(small.Count));
    }

    [Theory]
    [InlineData(0, null, null)]
    [InlineData(1, 3, "2026-12-01")] // Count and Until together
    [InlineData(1, 0, null)]
    public void Invalid_Rules_Are_Rejected(int interval, int? count, string? until)
    {
        var rule = new RecurrenceRule(RecurrenceFrequency.Daily, interval, Count: count, Until: until is null ? null : DateOnly.Parse(until));
        Assert.True(rule.Validate(new DateOnly(2026, 10, 15)).IsFailure);
    }

    [Fact]
    public void ByWeekday_On_Daily_Is_Rejected() =>
        Assert.True(new RecurrenceRule(RecurrenceFrequency.Daily, 1, [DayOfWeek.Monday]).Validate(new DateOnly(2026, 10, 15)).IsFailure);
}
