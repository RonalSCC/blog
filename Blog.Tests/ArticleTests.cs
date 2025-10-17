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
        var caller = () => When(new InitArticle(title, [], [], [new object()]));

        caller.Should().ThrowExactly<InitArticleException>().WithMessage(Article.EL_TITULO_NO_PUEDE_SER_VACIO);
    }

    [Fact]
    public void Si_CreoUnArticuloSinBloques_Debe_GenerarUnaExcepcion()
    {
        var caller = () => When(new InitArticle("Articulo de testing", [], [], [new object()]));

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

        var caller = () => When(new InitArticle("Articulo de testing", bloques, [], [new object()]));

        caller.Should().ThrowExactly<InitArticleException>().WithMessage(Article.NO_PUEDE_TENER_MAS_DE_20_BLOQUES);
    }

    [Fact]
    public void Si_CreoUnArticuloSinAutores_Debe_GenerarUnaExcepcion()
    {
        var caller = () => When(new InitArticle("Articulo de testing", [new object()], [], [new object()]));;
        
        caller.Should().ThrowExactly<InitArticleException>().WithMessage(Article.DEBE_CONTENER_AL_MENOS_UN_AUTOR);
    }

    [Fact]
    public void Si_CreoUnArticuloSinTags_Debe_RechazarLaCreacion()
    {
        var caller = () => When(new InitArticle("Articulo de testing", [new object()], [new object()], Tags: new List<object>() ));

        caller.Should().ThrowExactly<InitArticleException>()
            .WithMessage(Article.DEBE_CONTENER_AL_MENOS_UN_TAG_DESCRIPTIVO);
    }
}

public class Article
{
    public const string NO_PUEDE_TENER_MAS_DE_20_BLOQUES = "El articulo no puede contener más de 20 bloques";
    public const string DEBE_CONTENER_AL_MENOS_UN_BLOQUE = "No se puede crear un articulo sin bloques.";
    public const string EL_TITULO_NO_PUEDE_SER_VACIO = "No se puede crear un articulo con el titulo vacío.";
    public const string DEBE_CONTENER_AL_MENOS_UN_AUTOR = "El articulo debe contener al menos un autor.";
    public const string DEBE_CONTENER_AL_MENOS_UN_TAG_DESCRIPTIVO = "El articulo debe contener al menos un tag descriptivo.";
}

public class InitArticleException(string message) : Exception(message);

public class InitArticleCommandHandler(IEventStore eventStore) : ICommandHandler<InitArticle>
{
    public void Handle(InitArticle command)
    {
        AssertIfTitleIsEmpty(command.Title);

        AssertIfLengthOfBlocksIsCorrect(command.Block);

        AssertIfLengthOfAuthorsIsCorrect(command.Authors);

        throw new InitArticleException(Article.DEBE_CONTENER_AL_MENOS_UN_TAG_DESCRIPTIVO);
    }

    private static void AssertIfTitleIsEmpty(string title)
    {
        if (string.IsNullOrEmpty(title))
            throw new InitArticleException(Article.EL_TITULO_NO_PUEDE_SER_VACIO);
    }
    private static void AssertIfLengthOfBlocksIsCorrect(List<object> blocks)
    {
        if(blocks.Count == 0)
            throw new InitArticleException(Article.DEBE_CONTENER_AL_MENOS_UN_BLOQUE);
        
        if(blocks.Count > 20)
            throw new InitArticleException(Article.NO_PUEDE_TENER_MAS_DE_20_BLOQUES);
    }
    private static void AssertIfLengthOfAuthorsIsCorrect(List<object> authors)
    {
        if(authors.Count == 0)
            throw new InitArticleException(Article.DEBE_CONTENER_AL_MENOS_UN_AUTOR);
    }
}

public record InitArticle(string Title, List<object> Block, List<object> Authors, List<object> Tags);