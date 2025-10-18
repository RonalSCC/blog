namespace Blog.Dominio.Abstractions.EDA;

public interface IEventSender
{
    /// <summary>
    /// Publica un evento (EDA) asíncrono.
    /// </summary>
    /// <param name="event"></param>
    /// <returns></returns>
    public Task PublishEventAsync(IPublicEvent @event);
}