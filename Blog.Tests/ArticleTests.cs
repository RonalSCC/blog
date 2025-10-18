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
        var caller = () => When(new InitArticle(
            _aggregateId, 
            title, 
            [], 
            [], 
            [new object()],
            DateTime.UtcNow));

        caller.Should()
            .ThrowExactly<InitArticleException>().WithMessage(Article.EL_TITULO_NO_PUEDE_SER_VACIO);
    }

    [Fact]
    public void Si_CreoUnArticuloSinBloques_Debe_GenerarUnaExcepcion()
    {
        var caller = () => When(new InitArticle(
            _aggregateId, 
            "Articulo de testing", 
            [], 
            [], 
            [new object()],
            DateTime.UtcNow));

        caller.Should()
            .ThrowExactly<InitArticleException>().WithMessage(Article.DEBE_CONTENER_AL_MENOS_UN_BLOQUE);
    }

    [Fact]
    public void Si_CreoUnArticuloConMasDe20Bloques_Debe_GenerarUnaExcepcion()
    {
        var bloques = new List<object>();
        for (int i = 0; i < 21; i++)
        {
            bloques.Add(new object());
        }

        var caller = () => When(new InitArticle(
            _aggregateId, 
            "Articulo de testing", 
            bloques, 
            [], 
            [new object()],
            DateTime.UtcNow));

        caller.Should()
            .ThrowExactly<InitArticleException>().WithMessage(Article.NO_PUEDE_TENER_MAS_DE_20_BLOQUES);
    }

    [Fact]
    public void Si_CreoUnArticuloSinAutores_Debe_GenerarUnaExcepcion()
    {
        var caller = () =>
            When(new InitArticle(
                _aggregateId, 
                "Articulo de testing", 
                [new object()], 
                [], 
                [new object()],
                DateTime.UtcNow));
        ;

        caller.Should()
            .ThrowExactly<InitArticleException>().WithMessage(Article.DEBE_CONTENER_AL_MENOS_UN_AUTOR);
    }

    [Fact]
    public void Si_CreoUnArticuloSinTags_Debe_RechazarLaCreacion()
    {
        var id = Guid.CreateVersion7();
        var caller = () => When(new InitArticle(
            _aggregateId, 
            "Articulo de testing", 
            [new object()], 
            [new object()],
            new List<object>(),
            DateTime.UtcNow));

        caller.Should().ThrowExactly<InitArticleException>()
            .WithMessage(Article.DEBE_CONTENER_AL_MENOS_UN_TAG_DESCRIPTIVO);
    }

    [Fact]
    public void Si_CreoUnArticulo_Debe_GenerarUnEventoDeArticuloCreadoConFechaDeCreacion()
    {
        var createdAt = DateTime.UtcNow;
        var command = new InitArticle(
            _aggregateId,
            "Articulo de testing", 
            [new object()], 
            [new object()], 
            [new object()],
            createdAt);

        When(command);

        Then(_aggregateId,
            new ArticleInitiated(_aggregateId, command.Title, command.Block, command.Authors, command.Tags, command.CreatedAt));
        And<Article, string>(art => art.Id, _aggregateId);
        And<Article, DateTime>(art => art.CreatedAt, createdAt);
    }
}

public record ArticleInitiated(
    string Id,
    string Title,
    List<object> Block,
    List<object> Authors,
    List<object> Tags,
    DateTime CreatedAt);

public class Article : AggregateRoot
{
    public const string NO_PUEDE_TENER_MAS_DE_20_BLOQUES = "El articulo no puede contener más de 20 bloques";
    public const string DEBE_CONTENER_AL_MENOS_UN_BLOQUE = "No se puede crear un articulo sin bloques.";
    public const string EL_TITULO_NO_PUEDE_SER_VACIO = "No se puede crear un articulo con el titulo vacío.";
    public const string DEBE_CONTENER_AL_MENOS_UN_AUTOR = "El articulo debe contener al menos un autor.";

    public const string DEBE_CONTENER_AL_MENOS_UN_TAG_DESCRIPTIVO =
        "El articulo debe contener al menos un tag descriptivo.";

    public DateTime CreatedAt { get; private set; }

    public void Apply(ArticleInitiated @event)
    {
        Id = @event.Id;
        CreatedAt = @event.CreatedAt;
    }
}

public class InitArticleException(string message) : Exception(message);

public class InitArticleCommandHandler(IEventStore eventStore) : ICommandHandler<InitArticle>
{
    public void Handle(InitArticle command)
    {
        AssertIfTitleIsEmpty(command.Title);

        AssertIfLengthOfBlocksIsCorrect(command.Block);

        AssertIfLengthOfAuthorsIsCorrect(command.Authors);

        AssertTagLengthIsCorrect(command);

        eventStore.AppendEvent(command.Id,
            new ArticleInitiated(command.Id, command.Title, command.Block, command.Authors, command.Tags, command.CreatedAt));
    }

    private static void AssertTagLengthIsCorrect(InitArticle command)
    {
        if (command.Tags.Count == 0)
            throw new InitArticleException(Article.DEBE_CONTENER_AL_MENOS_UN_TAG_DESCRIPTIVO);
    }

    private static void AssertIfTitleIsEmpty(string title)
    {
        if (string.IsNullOrEmpty(title))
            throw new InitArticleException(Article.EL_TITULO_NO_PUEDE_SER_VACIO);
    }

    private static void AssertIfLengthOfBlocksIsCorrect(List<object> blocks)
    {
        if (blocks.Count == 0)
            throw new InitArticleException(Article.DEBE_CONTENER_AL_MENOS_UN_BLOQUE);

        if (blocks.Count > 20)
            throw new InitArticleException(Article.NO_PUEDE_TENER_MAS_DE_20_BLOQUES);
    }

    private static void AssertIfLengthOfAuthorsIsCorrect(List<object> authors)
    {
        if (authors.Count == 0)
            throw new InitArticleException(Article.DEBE_CONTENER_AL_MENOS_UN_AUTOR);
    }
}

public record InitArticle(
    string Id,
    string Title,
    List<object> Block,
    List<object> Authors,
    List<object> Tags,
    DateTime CreatedAt);