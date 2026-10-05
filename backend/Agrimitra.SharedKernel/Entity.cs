namespace Agrimitra.SharedKernel;

public interface IDomainEvent
{
    DateTimeOffset OccurredAt { get; }
}

public abstract class Entity<TId> where TId : notnull
{
    protected Entity(TId id) => Id = id;
    public TId Id { get; }
}

public abstract class AggregateRoot<TId> : Entity<TId> where TId : notnull
{
    private readonly List<IDomainEvent> _events = [];

    protected AggregateRoot(TId id) : base(id) { }

    public IReadOnlyList<IDomainEvent> DomainEvents => _events;
    public void ClearDomainEvents() => _events.Clear();
    protected void Raise(IDomainEvent e) => _events.Add(e);
}
