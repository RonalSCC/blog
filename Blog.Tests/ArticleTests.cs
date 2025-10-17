using AwesomeAssertions;
using Blog.Tests.Utilities;

namespace Blog.Tests;

public class ArticleTests : CommandHandlerTest<InitArticle>
{
    protected override ICommandHandler<InitArticle> Handler => new InitArticleCommandHandler(eventStore);
    
    [Fact]
    public void Si_CreoUnArticuloConTituloVacio_Debe_GenerarUnaExcepcion()
    {
        Given();
        
        var caller = () => When(new InitArticle(""));
        
        caller.Should().ThrowExactly<InitArticleException>().WithMessage("El titulo no puede estar vacío.");
    }

}

public class InitArticleException(string message) : Exception(message);

public class InitArticleCommandHandler(IEventStore eventStore) : ICommandHandler<InitArticle>
{
    private const string EL_TITULO_NO_PUEDE_SER_VACIO = "El titulo no puede estar vacío.";

    public void Handle(InitArticle command)
    {
        throw new InitArticleException(EL_TITULO_NO_PUEDE_SER_VACIO);
    }
}

public record InitArticle(string Title);