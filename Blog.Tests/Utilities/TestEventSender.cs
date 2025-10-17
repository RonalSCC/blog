using Blog.Tests.Abstractions;

namespace Blog.Tests.Utilities;

public class TestEventSender: IEventSender
{
    public Task PublishEventAsync(IPublicEvent @event)
    {
        return Task.CompletedTask;
    }
}