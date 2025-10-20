using AwesomeAssertions;
using Blog.Abstractions.EventSourcing;
using Blog.Dominio.Articles;
using Blog.Dominio.Articles.CommandHandlers;
using Blog.Dominio.Articles.Exceptions;
using Blog.Tests.Utilities;

namespace Blog.Tests.Article;

public class AddBlockTests : CommandHandlerTestAsync<ArticleCommands.AddBlock>
{
    protected override ICommandHandlerAsync<ArticleCommands.AddBlock> Handler => new AddBlockCommandHandler(eventStore);

    [Fact]
    public void Si_GuardanUnBloqueConContenidoVacio_Debe_GenerarExcepcion()
    {
        Given(new ArticleEvents.ArticleInitiated(_aggregateId, 
            "My fisrt article", [new object()], [new object()], [new object()], DateTime.UtcNow));
        
        var caller = () => When(new ArticleCommands.AddBlock(_aggregateId, ""));
        
        caller.Should().ThrowAsync<AddBlockException>().WithMessage(Block.NOT_ADD_BLOCK_WITHOUT_CONTENT);
    }
    
    [Fact]
    public void Si_GuardarUnBloqueAUnArticuloNoIniciado_Debe_GenerarExcepcion()
    {
        var caller = () => When(new ArticleCommands.AddBlock(Guid.CreateVersion7().ToString(), "Mi firts block"));
        
        caller.Should().ThrowAsync<AddBlockException>().WithMessage(Block.MUST_EXISTS_AN_ARTICLE_TO_ADD_BLOCK);
    }
}