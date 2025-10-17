using AwesomeAssertions;
using Blog.Tests.Utilities;

namespace Blog.Tests;

public class ArticleTests : CommandHandlerTest<InitArticle>
{
    protected override ICommandHandler<InitArticle> Handler => new InitArticleCommandHandler(eventStore);

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Si_CreoUnArticuloConTituloVacio_Debe_GenerarUnaExcepcion(string? title)
    {
        var caller = () => When(new InitArticle(title, null));

        caller.Should().ThrowExactly<InitArticleException>().WithMessage(Article.EL_TITULO_NO_PUEDE_SER_VACIO);
    }

    [Fact]
    public void Si_CreoUnArticuloSinBloques_Debe_GenerarUnaExcepcion()
    {
        var caller = () => When(new InitArticle("Articulo de testing", []));

        caller.Should().ThrowExactly<InitArticleException>().WithMessage(Article.DEBE_CONTENER_AL_MENOS_UN_BLOQUE);
    }
}

public class Article
{
    public static string DEBE_CONTENER_AL_MENOS_UN_BLOQUE = "No se puede crear un articulo sin bloques.";
    public const string EL_TITULO_NO_PUEDE_SER_VACIO = "No se puede crear un articulo con el titulo vacío.";
}

public class InitArticleException(string message) : Exception(message);

public class InitArticleCommandHandler(IEventStore eventStore) : ICommandHandler<InitArticle>
{
    public void Handle(InitArticle command)
    {
        if (string.IsNullOrEmpty(command.Title))
            throw new InitArticleException(Article.EL_TITULO_NO_PUEDE_SER_VACIO);

        throw new InitArticleException(Article.DEBE_CONTENER_AL_MENOS_UN_BLOQUE);
    }
}

public record InitArticle(string Title, List<object> Block);