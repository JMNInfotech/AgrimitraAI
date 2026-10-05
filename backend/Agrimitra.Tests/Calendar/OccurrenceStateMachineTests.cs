using Agrimitra.Domain.Calendar;

namespace Agrimitra.Tests.Calendar;

public class OccurrenceStateMachineTests
{
    public static IEnumerable<object[]> Terminal() =>
        new[] { OccurrenceStatus.Completed, OccurrenceStatus.Skipped, OccurrenceStatus.Cancelled, OccurrenceStatus.Rescheduled }
            .Select(s => new object[] { s });

    [Theory, MemberData(nameof(Terminal))]
    public void Terminal_States_Allow_No_Action(OccurrenceStatus s)
    {
        foreach (var a in Enum.GetValues<OccurrenceAction>())
            Assert.False(OccurrenceStateMachine.TryTransition(s, a, out _));
    }

    [Theory]
    [InlineData(OccurrenceStatus.Planned, OccurrenceAction.Confirm, OccurrenceStatus.Scheduled)]
    [InlineData(OccurrenceStatus.Scheduled, OccurrenceAction.MarkUpcoming, OccurrenceStatus.Upcoming)]
    [InlineData(OccurrenceStatus.Upcoming, OccurrenceAction.Start, OccurrenceStatus.InProgress)]
    [InlineData(OccurrenceStatus.InProgress, OccurrenceAction.Complete, OccurrenceStatus.Completed)]
    [InlineData(OccurrenceStatus.Scheduled, OccurrenceAction.Reschedule, OccurrenceStatus.Rescheduled)]
    [InlineData(OccurrenceStatus.InProgress, OccurrenceAction.Cancel, OccurrenceStatus.Cancelled)]
    public void Allowed_Transitions(OccurrenceStatus from, OccurrenceAction act, OccurrenceStatus to)
    {
        Assert.True(OccurrenceStateMachine.TryTransition(from, act, out var next));
        Assert.Equal(to, next);
    }

    [Fact]
    public void InProgress_Cannot_Be_Rescheduled() =>
        Assert.False(OccurrenceStateMachine.TryTransition(OccurrenceStatus.InProgress, OccurrenceAction.Reschedule, out _));
}
