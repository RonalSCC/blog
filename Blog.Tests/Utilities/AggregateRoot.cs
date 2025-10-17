namespace Blog.Tests.Utilities;

public abstract class AggregateRoot
{
    protected readonly List<object> _uncommittedEvents = new();
    public Guid Id { get; protected set; }
    public IReadOnlyList<object> UncommittedEvents => _uncommittedEvents.AsReadOnly();

    public void ClearUncommittedEvents()
    {
        _uncommittedEvents.Clear();
    }

    public void Apply(object eventData)
    {
    }
}