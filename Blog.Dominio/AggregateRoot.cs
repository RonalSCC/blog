namespace Blog.Dominio;

public abstract class AggregateRoot
{
    protected readonly List<object> _uncommittedEvents = new();
    public string Id { get; protected set; }
    public IReadOnlyList<object> UncommittedEvents => _uncommittedEvents.AsReadOnly();

    public void ClearUncommittedEvents()
    {
        _uncommittedEvents.Clear();
    }

    public void Apply(object eventData)
    {
    }
}