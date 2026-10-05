using Agrimitra.Domain.Calendar;

namespace Agrimitra.Tests.Calendar;

internal static class Fx
{
    public static readonly Guid Farmer = Guid.NewGuid();
    public static readonly Guid Consultant = Guid.NewGuid();
    public static readonly Guid OtherConsultant = Guid.NewGuid();
    public static readonly DateTimeOffset Now = new(2026, 10, 1, 0, 0, 0, TimeSpan.Zero);
    public const string Ist = "Asia/Kolkata";

    public static Schedule OneTime(int day = 15) => new(new DateOnly(2026, 10, day), new TimeOnly(7, 0), null, Ist);

    public static Schedule Recurring(RecurrenceRule rule) => new(new DateOnly(2026, 10, 15), new TimeOnly(8, 0), null, Ist, rule);

    public static ScheduledActivity Consultant_(Schedule? s = null, bool mandatory = true)
    {
        var a = ScheduledActivity.Create(Guid.NewGuid(), Farmer, Guid.NewGuid(), Guid.NewGuid(), ActivityType.Irrigation,
            "Irrigation", "Deep irrigation", ActivityPriority.High, mandatory, ActivityOrigin.Consultant, Consultant,
            Guid.NewGuid(), null, null, s ?? OneTime(), ActorContext.Consultant(Consultant), Now).Value;
        a.MaterializeOccurrences(Now.AddDays(90), Now);
        return a;
    }

    public static ScheduledActivity Farmer_(Schedule? s = null)
    {
        var a = ScheduledActivity.Create(Guid.NewGuid(), Farmer, Guid.NewGuid(), null, ActivityType.Weeding, "Weeding", null,
            ActivityPriority.Normal, false, ActivityOrigin.Farmer, null, null, null, null, s ?? OneTime(),
            ActorContext.Farmer(Farmer), Now).Value;
        a.MaterializeOccurrences(Now.AddDays(90), Now);
        return a;
    }
}
