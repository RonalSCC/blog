using AwesomeAssertions;
using Blog.Abstractions.EventSourcing;
using Blog.Dominio.Comandos;
using Blog.Dominio.CommandHandlers;
using Blog.Dominio.Events;
using Blog.Dominio.Exceptions;
using Blog.Tests.Utilities;

namespace Blog.Tests.Article;

public class InitArticleTests : CommandHandlerTest<ArticleCommands.InitArticle>
{
    protected override ICommandHandler<ArticleCommands.InitArticle> Handler => new InitArticleCommandHandler(eventStore);

    private readonly InitArticleBuilder _commandBuilder;

    public InitArticleTests()
    {
        _commandBuilder = new InitArticleBuilder().WithId(_aggregateId);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Si_CreoUnArticuloConTituloVacio_Debe_GenerarUnaExcepcion(string? title)
    {

        var command = _commandBuilder.WithTitle(title).Build();
        var caller = () => When(command);

        caller.Should()
            .ThrowExactly<InitArticleException>().WithMessage(Dominio.InitArticle.EL_TITULO_NO_PUEDE_SER_VACIO);
    }

    [Fact]
    public void Si_CreoUnArticuloSinBloques_Debe_GenerarUnaExcepcion()
    {
        var command = _commandBuilder.WithBlocks([]).Build();
        
        var caller = () => When(command);

        caller.Should()
            .ThrowExactly<InitArticleException>().WithMessage(Dominio.InitArticle.DEBE_CONTENER_AL_MENOS_UN_BLOQUE);
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
            .ThrowExactly<InitArticleException>().WithMessage(Dominio.InitArticle.NO_PUEDE_TENER_MAS_DE_20_BLOQUES);
    }

    [Fact]
    public void Si_CreoUnArticuloSinAutores_Debe_GenerarUnaExcepcion()
    {
        var command = _commandBuilder.WithAuthors([]).Build();
        
        var caller = () => When(command);

        caller.Should()
            .ThrowExactly<InitArticleException>().WithMessage(Dominio.InitArticle.DEBE_CONTENER_AL_MENOS_UN_AUTOR);
    }

    [Fact]
    public void Si_CreoUnArticuloSinTags_Debe_RechazarLaCreacion()
    {
        var command = _commandBuilder.WithTags([]).Build();
        
        var caller = () => When(command);

        caller.Should().ThrowExactly<InitArticleException>()
            .WithMessage(Dominio.InitArticle.DEBE_CONTENER_AL_MENOS_UN_TAG_DESCRIPTIVO);
    }

    [Fact]
    public void Si_CreoUnArticulo_Debe_GenerarUnEventoDeArticuloCreadoConFechaDeCreacion()
    {
        var createdAt = DateTime.UtcNow;
        var command = _commandBuilder.WithCreatedAt(createdAt).Build();
        
        When(command);

        Then(_aggregateId,
            new ArticleEvents.ArticleInitiated(_aggregateId, command.Title, command.Block, command.Authors, command.Tags, command.CreatedAt));
        And<Dominio.InitArticle, string>(art => art.Id, _aggregateId);
        And<Dominio.InitArticle, DateTime>(art => art.CreatedAt, createdAt);
    }
}