namespace Agrimitra.Domain.Calendar;

/// <summary>Who is performing a command. Built by the application layer from the authenticated principal.</summary>
public sealed record ActorContext(Guid UserId, ActorKind Kind, string? AdminReason = null)
{
    public static ActorContext Farmer(Guid id) => new(id, ActorKind.Farmer);
    public static ActorContext Consultant(Guid id) => new(id, ActorKind.Consultant);
    public static ActorContext Admin(Guid id, string reason) => new(id, ActorKind.Admin, reason);
    public static ActorContext Ai(Guid serviceId) => new(serviceId, ActorKind.AiService);
    public static ActorContext System() => new(Guid.Empty, ActorKind.System);
}
