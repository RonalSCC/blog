using Blog.Abstractions.EventSourcing;

namespace Blog.Tests.Utilities;

public abstract class CommandHandlerTest<TCommand> : CommandHandlerTestBase
{
    /// <summary>
    /// The command handler, to be provided in the Test class.
    /// This to account for additional injections
    /// </summary>
    protected abstract ICommandHandler<TCommand> Handler { get; }

    /// <summary>
    /// Triggers the handling of a command against the configured events.
    /// </summary>
    protected void When(TCommand command)
    {
        Handler.Handle(command);
    }
}

public abstract class CommandHandlerTestAsync<TCommand> : CommandHandlerTestBase
{
    /// <summary>
    /// The command handler, to be provided in the Test class.
    /// This to account for additional injections
    /// </summary>
    protected abstract ICommandHandlerAsync<TCommand> Handler { get; }

    /// <summary>
    /// Triggers the handling of a command against the configured events.
    /// </summary>
    protected async Task When(TCommand command)
    {
        await Handler.HandleAsync(command, CancellationToken.None);
    }
}