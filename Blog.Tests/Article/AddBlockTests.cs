using AwesomeAssertions;
using Blog.Abstractions.EventSourcing;
using Blog.Dominio.Article;
using Blog.Tests.Utilities;

namespace Blog.Tests.Article;

public class AddBlockTests : CommandHandlerTest<ArticleCommands.AddBlock>
{
    protected override ICommandHandler<ArticleCommands.AddBlock> Handler => new AddBlockCommandHandler(eventStore);

    [Fact]
    public void Si_GuardanUnBloqueConContenidoVacio_Debe_GenerarExcepcion()
    {
        Given(new ArticleEvents.ArticleInitiated(_aggregateId, 
            "My fisrt article", [new object()], [new object()], [new object()], DateTime.UtcNow));
        
        var caller = () => When(new ArticleCommands.AddBlock());
        
        caller.Should().Throw<AddBlockException>().WithMessage(Block.NOT_ADD_BLOCK_WITHOUT_CONTENT);
    }
}

public class Block
{
    public const string NOT_ADD_BLOCK_WITHOUT_CONTENT = "No se puede agregar un bloque sin contenido.";
}

public class AddBlockException(string message) : Exception(message) { }

public class AddBlockCommandHandler(IEventStore eventStore) : ICommandHandler<ArticleCommands.AddBlock>
{
    public void Handle(ArticleCommands.AddBlock command)
    {
        throw new AddBlockException(Block.NOT_ADD_BLOCK_WITHOUT_CONTENT);
    }
}