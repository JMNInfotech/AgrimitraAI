using Agrimitra.Domain.Calendar;

namespace Agrimitra.Tests.Calendar;

public class ScheduledActivityTests
{
    private static readonly DateTimeOffset NewTime = Fx.Now.AddDays(20);

    [Fact]
    public void Consultant_Activity_Can_Only_Be_Created_By_The_Owning_Consultant()
    {
        var r = ScheduledActivity.Create(Guid.NewGuid(), Fx.Farmer, Guid.NewGuid(), null, ActivityType.Pesticide, "x", null,
            ActivityPriority.Normal, true, ActivityOrigin.Consultant, Fx.Consultant, null, null, null, Fx.OneTime(),
            ActorContext.Consultant(Fx.OtherConsultant), Fx.Now);
        Assert.Equal(CalendarErrors.Forbidden, r.Error);
    }

    [Fact]
    public void Ai_Cannot_Create_Activities()
    {
        var r = ScheduledActivity.Create(Guid.NewGuid(), Fx.Farmer, Guid.NewGuid(), null, ActivityType.Irrigation, "x", null,
            ActivityPriority.Normal, false, ActivityOrigin.AiAccepted, null, null, null, Guid.NewGuid(), Fx.OneTime(),
            ActorContext.Ai(Guid.NewGuid()), Fx.Now);
        Assert.True(r.IsFailure);
    }

    [Fact]
    public void Materialize_Is_Idempotent()
    {
        var a = Fx.Consultant_(Fx.Recurring(new(RecurrenceFrequency.Daily, 1)));
        var count = a.Occurrences.Count;
        Assert.Equal(0, a.MaterializeOccurrences(Fx.Now.AddDays(90), Fx.Now));
        Assert.Equal(count, a.Occurrences.Count);
        Assert.True(a.MaterializeOccurrences(Fx.Now.AddDays(120), Fx.Now) > 0);
    }

    [Theory]
    [MemberData(nameof(ForbiddenActors))]
    public void Only_Owning_Consultant_Can_Reschedule_Consultant_Activity(ActorContext actor)
    {
        var a = Fx.Consultant_();
        var r = a.Reschedule(a.Occurrences[0].Id, NewTime, null, actor, ChangeVia.Direct, null, Fx.Now);
        Assert.Equal(CalendarErrors.Forbidden, r.Error);
        Assert.Equal(OccurrenceStatus.Scheduled, a.Occurrences[0].Status);
    }

    public static IEnumerable<object[]> ForbiddenActors() =>
    [
        [ActorContext.Farmer(Fx.Farmer)],
        [ActorContext.Consultant(Fx.OtherConsultant)],
        [ActorContext.Ai(Guid.NewGuid())],
        [ActorContext.System()],
        [new ActorContext(Guid.NewGuid(), ActorKind.Admin, " ")], // admin without a reason
    ];

    [Fact]
    public void Owning_Consultant_Reschedule_Creates_New_Occurrence_And_History()
    {
        var a = Fx.Consultant_();
        var old = a.Occurrences[0];
        var r = a.Reschedule(old.Id, NewTime, null, ActorContext.Consultant(Fx.Consultant), ChangeVia.Direct, "rain", Fx.Now);
        Assert.True(r.IsSuccess);
        Assert.Equal(OccurrenceStatus.Rescheduled, old.Status);
        var fresh = a.Occurrences.Single(o => o.Id == old.RescheduledToOccurrenceId);
        Assert.Equal(NewTime, fresh.StartUtc);
        Assert.Equal(old.SeqNo, fresh.SeqNo);
        Assert.Equal(1, fresh.Revision);
        Assert.Contains(a.History, h => h.Type == ChangeType.Rescheduled && h.Reason == "rain" && h.ActorUserId == Fx.Consultant);
        Assert.IsType<ActivityRescheduled>(a.DomainEvents.Last());
    }

    [Fact]
    public void Admin_With_Reason_Can_Override()
    {
        var a = Fx.Consultant_();
        var r = a.Cancel(a.Occurrences[0].Id, ActorContext.Admin(Guid.NewGuid(), "consultant unreachable"), "x", Fx.Now);
        Assert.True(r.IsSuccess);
    }

    [Fact]
    public void Reschedule_To_The_Past_Is_Rejected()
    {
        var a = Fx.Consultant_();
        var r = a.Reschedule(a.Occurrences[0].Id, Fx.Now.AddDays(-1), null, ActorContext.Consultant(Fx.Consultant), ChangeVia.Direct, null, Fx.Now);
        Assert.Equal(CalendarErrors.PastTime, r.Error);
    }

    [Fact]
    public void Farmer_Completes_With_Evidence_And_Snapshot()
    {
        var a = Fx.Consultant_();
        var id = a.Occurrences[0].Id;
        var r = a.Complete(id, ActorContext.Farmer(Fx.Farmer), "done", ["evidence/1.jpg"], Fx.Now.AddDays(14), Fx.Now.AddDays(14));
        Assert.True(r.IsSuccess);
        var c = a.Occurrences[0].Completion!;
        Assert.Equal("Deep irrigation", c.InstructionsSnapshot);
        Assert.Equal(a.Occurrences[0].StartUtc, c.ScheduledAtUtc);
        Assert.Equal(["evidence/1.jpg"], c.EvidenceObjectKeys);
        Assert.IsType<ActivityCompleted>(a.DomainEvents.Last());
    }

    [Fact]
    public void Consultant_Cannot_Mark_Farmer_Work_As_Completed()
    {
        var a = Fx.Consultant_();
        Assert.Equal(CalendarErrors.Forbidden,
            a.Complete(a.Occurrences[0].Id, ActorContext.Consultant(Fx.Consultant), null, null, Fx.Now, Fx.Now).Error);
    }

    [Fact]
    public void Completed_Occurrence_Cannot_Be_Completed_Or_Rescheduled_Again()
    {
        var a = Fx.Consultant_();
        var id = a.Occurrences[0].Id;
        a.Complete(id, ActorContext.Farmer(Fx.Farmer), null, null, Fx.Now, Fx.Now);
        Assert.Equal(CalendarErrors.InvalidTransition, a.Complete(id, ActorContext.Farmer(Fx.Farmer), null, null, Fx.Now, Fx.Now).Error);
        Assert.Equal(CalendarErrors.InvalidTransition,
            a.Reschedule(id, NewTime, null, ActorContext.Consultant(Fx.Consultant), ChangeVia.Direct, null, Fx.Now).Error);
    }

    [Fact]
    public void Skipping_Mandatory_Consultant_Activity_Requires_Approval()
    {
        var a = Fx.Consultant_(mandatory: true);
        var r = a.Skip(a.Occurrences[0].Id, ActorContext.Farmer(Fx.Farmer), "no water", false, Fx.Now);
        Assert.Equal(CalendarErrors.ApprovalRequired, r.Error);
        Assert.Equal(OccurrenceStatus.Scheduled, a.Occurrences[0].Status);
    }

    [Fact]
    public void Skipping_Optional_Consultant_Activity_Needs_Reason_And_Is_Recorded()
    {
        var a = Fx.Consultant_(mandatory: false);
        var id = a.Occurrences[0].Id;
        Assert.Equal(CalendarErrors.ReasonRequired, a.Skip(id, ActorContext.Farmer(Fx.Farmer), " ", false, Fx.Now).Error);
        Assert.True(a.Skip(id, ActorContext.Farmer(Fx.Farmer), "rain", true, Fx.Now).IsSuccess);
        Assert.Equal(CompletionOutcome.NotApplicable, a.Occurrences[0].Completion!.Outcome);
    }

    [Fact]
    public void Farmer_Controls_Own_Activity()
    {
        var a = Fx.Farmer_();
        Assert.True(a.Reschedule(a.Occurrences[0].Id, NewTime, null, ActorContext.Farmer(Fx.Farmer), ChangeVia.Direct, null, Fx.Now).IsSuccess);
        Assert.Equal(CalendarErrors.Forbidden,
            a.Cancel(a.Occurrences.Last().Id, ActorContext.Consultant(Fx.Consultant), null, Fx.Now).Error);
    }

    [Fact]
    public void Other_Farmer_Cannot_Touch_Activity()
    {
        var a = Fx.Farmer_();
        Assert.Equal(CalendarErrors.Forbidden,
            a.Complete(a.Occurrences[0].Id, ActorContext.Farmer(Guid.NewGuid()), null, null, Fx.Now, Fx.Now).Error);
    }

    [Fact]
    public void Missed_Is_Computed_Not_A_Status()
    {
        var a = Fx.Consultant_();
        Assert.False(a.Occurrences[0].IsMissed(Fx.Now));
        Assert.True(a.Occurrences[0].IsMissed(Fx.Now.AddDays(30)));
    }

    [Fact]
    public void MarkUpcoming_Only_Within_Window()
    {
        var a = Fx.Consultant_();
        Assert.Equal(0, a.MarkUpcoming(Fx.Now, TimeSpan.FromHours(24)));
        Assert.Equal(1, a.MarkUpcoming(a.Occurrences[0].StartUtc.AddHours(-23), TimeSpan.FromHours(24)));
        Assert.Equal(OccurrenceStatus.Upcoming, a.Occurrences[0].Status);
    }
}
