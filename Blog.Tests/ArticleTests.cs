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
    
    [Fact]
    public void Si_CreoUnArticuloConMasDe20Bloques_Debe_GenerarUnaExcepcion()
    {
        var bloques = new List<object>();
        for (int i = 0; i < 21; i++)
        {
            bloques.Add(new object());
        }

        var caller = () => When(new InitArticle("Articulo de testing", bloques));

        caller.Should().ThrowExactly<InitArticleException>().WithMessage(Article.NO_PUEDE_TENER_MAS_DE_20_BLOQUES);
    }
}

public class Article
{
    public const string NO_PUEDE_TENER_MAS_DE_20_BLOQUES = "El articulo no puede contener más de 20 bloques";
    public const string DEBE_CONTENER_AL_MENOS_UN_BLOQUE = "No se puede crear un articulo sin bloques.";
    public const string EL_TITULO_NO_PUEDE_SER_VACIO = "No se puede crear un articulo con el titulo vacío.";
}

public class InitArticleException(string message) : Exception(message);

public class InitArticleCommandHandler(IEventStore eventStore) : ICommandHandler<InitArticle>
{
    public void Handle(InitArticle command)
    {
        if (IsAInvalidTitle(command))
            throw new InitArticleException(Article.EL_TITULO_NO_PUEDE_SER_VACIO);

        if(command.Block.Count == 0)
            throw new InitArticleException(Article.DEBE_CONTENER_AL_MENOS_UN_BLOQUE);
        
        throw new InitArticleException(Article.NO_PUEDE_TENER_MAS_DE_20_BLOQUES);
        
    }

    private static bool IsAInvalidTitle(InitArticle command)
    {
        return string.IsNullOrEmpty(command.Title);
    }
}

public record InitArticle(string Title, List<object> Block);