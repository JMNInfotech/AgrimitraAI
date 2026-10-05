using Agrimitra.Domain.Calendar;

namespace Agrimitra.Tests.Calendar;

public class ScheduleChangeProposalTests
{
    private static ScheduleChangeProposal Ai(ScheduledActivity a) =>
        ScheduleChangeProposal.Create(Guid.NewGuid(), a.Id, a.Occurrences[0].Id, Fx.Now.AddDays(20), "Heavy rain forecast", ProposalRaisedBy.Ai);

    [Fact]
    public void Creating_A_Proposal_Never_Changes_The_Schedule()
    {
        var a = Fx.Consultant_();
        var before = a.Occurrences[0].StartUtc;
        _ = Ai(a);
        Assert.Equal(before, a.Occurrences[0].StartUtc);
        Assert.Equal(OccurrenceStatus.Scheduled, a.Occurrences[0].Status);
    }

    [Fact]
    public void Consultant_Approval_Reschedules_Via_Proposal_And_Is_Audited()
    {
        var a = Fx.Consultant_();
        var p = Ai(a);
        Assert.True(p.Approve(a, ActorContext.Consultant(Fx.Consultant), "ok", Fx.Now).IsSuccess);
        Assert.Equal(ProposalStatus.Approved, p.Status);
        Assert.Equal(OccurrenceStatus.Rescheduled, a.Occurrences[0].Status);
        Assert.Contains(a.History, h => h.Type == ChangeType.Rescheduled && h.Via == ChangeVia.AiProposalApproval);
        Assert.Contains(a.History, h => h.Type == ChangeType.ProposalApproved);
    }

    [Theory]
    [MemberData(nameof(Cannot))]
    public void Only_The_Owning_Consultant_Can_Decide_On_Consultant_Activity(ActorContext who)
    {
        var a = Fx.Consultant_();
        var p = Ai(a);
        Assert.Equal(CalendarErrors.Forbidden, p.Approve(a, who, null, Fx.Now).Error);
        Assert.Equal(CalendarErrors.Forbidden, p.Reject(a, who, null, Fx.Now).Error);
        Assert.Equal(ProposalStatus.Pending, p.Status);
        Assert.Equal(OccurrenceStatus.Scheduled, a.Occurrences[0].Status);
    }

    public static IEnumerable<object[]> Cannot() =>
    [
        [ActorContext.Farmer(Fx.Farmer)],
        [ActorContext.Ai(Guid.NewGuid())],
        [ActorContext.System()],
        [ActorContext.Consultant(Fx.OtherConsultant)],
    ];

    [Fact]
    public void Rejection_Leaves_Schedule_Untouched()
    {
        var a = Fx.Consultant_();
        var p = Ai(a);
        Assert.True(p.Reject(a, ActorContext.Consultant(Fx.Consultant), "keep", Fx.Now).IsSuccess);
        Assert.Equal(OccurrenceStatus.Scheduled, a.Occurrences[0].Status);
        Assert.Contains(a.History, h => h.Type == ChangeType.ProposalRejected);
    }

    [Fact]
    public void Decided_Proposal_Cannot_Be_Decided_Again()
    {
        var a = Fx.Consultant_();
        var p = Ai(a);
        p.Reject(a, ActorContext.Consultant(Fx.Consultant), null, Fx.Now);
        Assert.Equal(CalendarErrors.ProposalNotPending, p.Approve(a, ActorContext.Consultant(Fx.Consultant), null, Fx.Now).Error);
    }

    [Fact]
    public void Farmer_Decides_Proposals_On_Own_Activities()
    {
        var a = Fx.Farmer_();
        var p = Ai(a);
        Assert.True(p.Approve(a, ActorContext.Farmer(Fx.Farmer), null, Fx.Now).IsSuccess);
    }

    [Fact]
    public void Expired_Proposal_Is_Not_Actionable()
    {
        var a = Fx.Consultant_();
        var p = Ai(a);
        p.Expire();
        Assert.Equal(CalendarErrors.ProposalNotPending, p.Approve(a, ActorContext.Consultant(Fx.Consultant), null, Fx.Now).Error);
    }
}
