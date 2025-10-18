namespace Blog.Abstractions.EventSourcing;

public interface IEventStore
{
    /// <summary>
    /// Agrega un evento al almacén de eventos para un agregado específico.
    /// </summary>
    /// <param name="aggregateId"></param>
    /// <param name="eventData"></param>
    void AppendEvent(Guid aggregateId, object eventData);
    
    /// <summary>
    /// Agrega un evento al almacén de eventos para un agregado específico utilizando su ID como cadena.
    /// </summary>
    /// <param name="aggregateId"></param>
    /// <param name="eventData"></param>
    void AppendEvent(string aggregateId, object eventData);
    
    /// <summary>
    /// Guarda los cambios realizados en el almacén de eventos de forma asíncrona.
    /// </summary>
    /// <returns></returns>
    Task SaveChangesAsync();
    
    /// <summary>
    /// Guarda un agregado raíz en el almacén de eventos.
    /// </summary>
    /// <param name="aggregateRoot"></param>
    void Save(AggregateRoot aggregateRoot);

    /// <summary>
    /// Obtiene un <see cref="Blog.Tests.Utilities.AggregateRoot"/> por su ID.
    /// </summary>
    /// <param name="aggregateId"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TAggregateRoot"></typeparam>
    /// <returns></returns>
    Task<TAggregateRoot?> GetAggregateRootAsync<TAggregateRoot>(string aggregateId,
        CancellationToken cancellationToken) where TAggregateRoot : AggregateRoot, new();
    
    /// <summary>
    /// Obtiene un <see cref="Blog.Tests.Utilities.AggregateRoot"/> por su ID como <see cref="Guid"/>.
    /// </summary>
    /// <param name="aggregateId"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TAggregateRoot"></typeparam>
    /// <returns></returns>
    Task<TAggregateRoot?> GetAggregateRootAsync<TAggregateRoot>(Guid aggregateId,
        CancellationToken cancellationToken) where TAggregateRoot : AggregateRoot, new();

    /// <summary>
    /// Obtiene un <see cref="Blog.Tests.Utilities.AggregateRoot"/> por su ID y versión.
    /// </summary>
    /// <param name="aggregateId"></param>
    /// <param name="version"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TAggregateRoot"></typeparam>
    /// <returns></returns>
    Task<TAggregateRoot?> GetAggregateRootAsync<TAggregateRoot>(string aggregateId,
        int version, CancellationToken cancellationToken) where TAggregateRoot : AggregateRoot, new();

    /// <summary>
    /// Obtiene un <see cref="Blog.Tests.Utilities.AggregateRoot"/> por su ID como <see cref="Guid"/> y versión.
    /// </summary>
    /// <param name="aggregateId"></param>
    /// <param name="version"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TAggregateRoot"></typeparam>
    /// <returns></returns>
    Task<TAggregateRoot?> GetAggregateRootAsync<TAggregateRoot>(Guid aggregateId,
        int version, CancellationToken cancellationToken) where TAggregateRoot : AggregateRoot, new();

    /// <summary>
    /// Obtiene un <see cref="Blog.Tests.Utilities.AggregateRoot"/> por su ID y un desplazamiento de tiempo opcional.
    /// </summary>
    /// <param name="aggregateId"></param>
    /// <param name="timeOffset"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TAggregateRoot"></typeparam>
    /// <returns></returns>
    Task<TAggregateRoot?> GetAggregateRootAsync<TAggregateRoot>(string aggregateId,
        DateTimeOffset? timeOffset, CancellationToken cancellationToken) where TAggregateRoot : AggregateRoot, new();

    /// <summary>
    /// Obtiene un <see cref="Blog.Tests.Utilities.AggregateRoot"/> por su ID como <see cref="Guid"/> y un desplazamiento de tiempo opcional.
    /// </summary>
    /// <param name="aggregateId"></param>
    /// <param name="timeOffset"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="TAggregateRoot"></typeparam>
    /// <returns></returns>
    Task<TAggregateRoot?> GetAggregateRootAsync<TAggregateRoot>(Guid aggregateId,
        DateTimeOffset? timeOffset, CancellationToken cancellationToken) where TAggregateRoot : AggregateRoot, new();
}