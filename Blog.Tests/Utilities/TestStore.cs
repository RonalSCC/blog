namespace Blog.Tests.Utilities;

public class TestStore : IEventStore
{
    private readonly Dictionary<string, List<object>> _newEvents = new();
    private readonly Dictionary<string, List<object>> _previousEvents = new();

    public void AppendEvent(Guid aggregateId, object eventData)
    {
        AppendEvent(aggregateId.ToString(), eventData);
    }

    public void AppendEvent(string aggregateId, object eventData)
    {
        var eventos = _newEvents.GetValueOrDefault(aggregateId, []);
        eventos.Add(eventData);
        _newEvents[aggregateId] = eventos;
    }

    public Task SaveChangesAsync()
    {
        throw new NotImplementedException();
    }

    public void Save(AggregateRoot aggregateRoot)
    {
        var eventos = _newEvents.GetValueOrDefault(aggregateRoot.Id.ToString(), []);
        eventos.AddRange(aggregateRoot.UncommittedEvents);
        _newEvents[aggregateRoot.Id.ToString()] = eventos;
        aggregateRoot.ClearUncommittedEvents();
    }

    public Task<TAggregateRoot?> GetAggregateRootAsync<TAggregateRoot>(Guid aggregateId,
        CancellationToken cancellationToken) where TAggregateRoot : AggregateRoot, new()
    {
        return GetAggregateRootAsync<TAggregateRoot>(aggregateId.ToString(), cancellationToken);
    }

    public Task<TAggregateRoot?> GetAggregateRootAsync<TAggregateRoot>(string aggregateId, int version, CancellationToken cancellationToken) where TAggregateRoot : AggregateRoot, new()
    {
        if (_previousEvents.ContainsKey(aggregateId) is false || _previousEvents[aggregateId].Any() is false)
            return Task.FromResult<TAggregateRoot?>(null);

        TAggregateRoot aggregateRoot = new();

        foreach (var @event in _previousEvents[aggregateId][..version])
        {
            ApplyEvent(aggregateRoot, @event);
        }

        return Task.FromResult(aggregateRoot)!;
    }

    public Task<TAggregateRoot?> GetAggregateRootAsync<TAggregateRoot>(Guid aggregateId, int version, CancellationToken cancellationToken) where TAggregateRoot : AggregateRoot, new()
    {
        return GetAggregateRootAsync<TAggregateRoot>(aggregateId.ToString(), version, cancellationToken);
    }

    public Task<TAggregateRoot?> GetAggregateRootAsync<TAggregateRoot>(string aggregateId, DateTimeOffset? timeOffset,
        CancellationToken cancellationToken) where TAggregateRoot : AggregateRoot, new()
    {
        throw new NotImplementedException();
    }

    public Task<TAggregateRoot?> GetAggregateRootAsync<TAggregateRoot>(Guid aggregateId, DateTimeOffset? timeOffset,
        CancellationToken cancellationToken) where TAggregateRoot : AggregateRoot, new()
    {
        throw new NotImplementedException();
    }

    public Task<TAggregateRoot?> GetAggregateRootAsync<TAggregateRoot>(string aggregateId,
        CancellationToken cancellationToken)
        where TAggregateRoot : AggregateRoot, new()
    {
        if (_previousEvents.ContainsKey(aggregateId) is false || _previousEvents[aggregateId].Any() is false)
            return Task.FromResult<TAggregateRoot?>(null);

        TAggregateRoot aggregateRoot = new();

        foreach (var @event in _previousEvents[aggregateId])
        {
            ApplyEvent(aggregateRoot, @event);
        }

        return Task.FromResult(aggregateRoot)!;
    }


    public void AppendPreviosEvents(string aggregateId, object[] events)
    {
        _previousEvents.Add(aggregateId, events.ToList());
    }

    public IEnumerable<object> GetNewEvents(string aggregateId)
    {
        return _newEvents.GetValueOrDefault(aggregateId, []);
    }

    public TAggregateRoot? GetAggregateRoot<TAggregateRoot>(string aggregateId)
        where TAggregateRoot : AggregateRoot, new()
    {
        if (_previousEvents.ContainsKey(aggregateId) is false &&
            _newEvents.ContainsKey(aggregateId) is false)
            return null;

        TAggregateRoot aggregateRoot = new();

        foreach (var @event in (List<object>)
                 [
                     .._previousEvents.GetValueOrDefault(aggregateId, []),
                     .._newEvents.GetValueOrDefault(aggregateId, [])
                 ])
            ApplyEvent(aggregateRoot, @event);

        return aggregateRoot;
    }

    private static void ApplyEvent<TAggregateRoot>(TAggregateRoot aggregateRoot, object @event)
        where TAggregateRoot : AggregateRoot, new()
    {
        var applyMethod = aggregateRoot.GetType().GetMethods().FirstOrDefault(m =>
            m.Name == "Apply" &&
            m.GetParameters().Length == 1 &&
            m.GetParameters()[0].ParameterType == @event.GetType()
        );

        if (applyMethod != null)
        {
            applyMethod.Invoke(aggregateRoot, new[] { @event });
        }
        else
        {
            Console.WriteLine($"No handler found for {@event.GetType()}");
        }
    }
}