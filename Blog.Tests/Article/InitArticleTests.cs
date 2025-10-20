using AwesomeAssertions;
using Blog.Abstractions.EventSourcing;
using Blog.Dominio.Articles;
using Blog.Dominio.Articles.CommandHandlers;
using Blog.Dominio.Articles.Exceptions;
using Blog.Tests.Utilities;

namespace Blog.Tests.Article;

public class ArticleTests : CommandHandlerTest<ArticleCommands.InitArticle>
{
    protected override ICommandHandler<ArticleCommands.InitArticle> Handler => new InitArticleCommandHandler(eventStore);

    private readonly InitArticleBuilder _commandBuilder;

    public ArticleTests()
    {
        _commandBuilder = new InitArticleBuilder().WithId(_aggregateId);
    }
    
    [Fact]
    public void Si_CreanUnArticuloConIdVacio_Debe_GenerarUnaExcepcion()
    {
        var command = _commandBuilder.WithId("").Build();
        var caller = () => When(command);

        caller.Should().ThrowExactly<InitArticleException>().WithMessage(Dominio.Articles.Article.EL_ID_NO_PUEDE_SER_VACIO);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Si_CreoUnArticuloConTituloVacio_Debe_GenerarUnaExcepcion(string? title)
    {

        var command = _commandBuilder.WithTitle(title).Build();
        var caller = () => When(command);

        caller.Should()
            .ThrowExactly<InitArticleException>().WithMessage(Dominio.Articles.Article.EL_TITULO_NO_PUEDE_SER_VACIO);
    }

    [Fact]
    public void Si_CreoUnArticuloSinBloques_Debe_GenerarUnaExcepcion()
    {
        var command = _commandBuilder.WithBlocks([]).Build();
        
        var caller = () => When(command);

        caller.Should()
            .ThrowExactly<InitArticleException>().WithMessage(Dominio.Articles.Article.DEBE_CONTENER_AL_MENOS_UN_BLOQUE);
    }

    [Fact]
    public void Si_CreoUnArticuloConMasDe20Bloques_Debe_GenerarUnaExcepcion()
    {
        var bloques = new List<object>();
        for (int i = 0; i < 21; i++)
        {
            bloques.Add(new object());
        }
        
        var command = _commandBuilder.WithBlocks(bloques).Build();
        
        var caller = () => When(command);

        caller.Should()
            .ThrowExactly<InitArticleException>().WithMessage(Dominio.Articles.Article.NO_PUEDE_TENER_MAS_DE_20_BLOQUES);
    }

    [Fact]
    public void Si_CreoUnArticuloSinAutores_Debe_GenerarUnaExcepcion()
    {
        var command = _commandBuilder.WithAuthors([]).Build();
        
        var caller = () => When(command);

        caller.Should()
            .ThrowExactly<InitArticleException>().WithMessage(Dominio.Articles.Article.DEBE_CONTENER_AL_MENOS_UN_AUTOR);
    }

    [Fact]
    public void Si_CreoUnArticuloSinTags_Debe_RechazarLaCreacion()
    {
        var command = _commandBuilder.WithTags([]).Build();
        
        var caller = () => When(command);

        caller.Should().ThrowExactly<InitArticleException>()
            .WithMessage(Dominio.Articles.Article.DEBE_CONTENER_AL_MENOS_UN_TAG_DESCRIPTIVO);
    }

    [Fact]
    public void Si_CreoUnArticulo_Debe_GenerarUnEventoDeArticuloCreadoConFechaDeCreacion()
    {
        var createdAt = DateTime.UtcNow;
        var command = _commandBuilder.WithCreatedAt(createdAt).Build();
        
        When(command);

        Then(_aggregateId,
            new ArticleEvents.ArticleInitiated(_aggregateId, command.Title, command.Block, command.Authors, command.Tags, command.CreatedAt));
        And<Dominio.Articles.Article, string>(art => art.Id, _aggregateId);
        And<Dominio.Articles.Article, string>(art => art.Title, command.Title);
        And<Dominio.Articles.Article, IReadOnlyList<object>>(art => art.Block, command.Block);
        And<Dominio.Articles.Article, IReadOnlyList<object>>(art => art.Authors, command.Authors);
        And<Dominio.Articles.Article, IReadOnlyList<object>>(art => art.Tags, command.Tags);
        And<Dominio.Articles.Article, DateTime>(art => art.CreatedAt, createdAt);
    }
    
    
}