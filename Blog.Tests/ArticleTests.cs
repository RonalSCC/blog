using AwesomeAssertions;
using Blog.Dominio;
using Blog.Dominio.Abstractions.EventSourcing;
using Blog.Dominio.Comandos;
using Blog.Dominio.CommandHandlers;
using Blog.Dominio.Events;
using Blog.Dominio.Exceptions;
using Blog.Tests.Utilities;

namespace Blog.Tests;

public class ArticleTests : CommandHandlerTest<ArticleCommands.InitArticle>
{
    protected override ICommandHandler<ArticleCommands.InitArticle> Handler => new InitArticleCommandHandler(eventStore);

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Si_CreoUnArticuloConTituloVacio_Debe_GenerarUnaExcepcion(string? title)
    {
        var caller = () => When(new ArticleCommands.InitArticle(
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
        var caller = () => When(new ArticleCommands.InitArticle(
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

        var caller = () => When(new ArticleCommands.InitArticle(
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
            When(new ArticleCommands.InitArticle(
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
        var caller = () => When(new ArticleCommands.InitArticle(
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
        var command = new ArticleCommands.InitArticle(
            _aggregateId,
            "Articulo de testing", 
            [new object()], 
            [new object()], 
            [new object()],
            createdAt);

        When(command);

        Then(_aggregateId,
            new ArticleEvents.ArticleInitiated(_aggregateId, command.Title, command.Block, command.Authors, command.Tags, command.CreatedAt));
        And<Article, string>(art => art.Id, _aggregateId);
        And<Article, DateTime>(art => art.CreatedAt, createdAt);
    }
}