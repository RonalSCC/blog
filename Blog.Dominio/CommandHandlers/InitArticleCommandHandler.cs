using Blog.Abstractions.EventSourcing;
using Blog.Dominio.Comandos;
using Blog.Dominio.Events;
using Blog.Dominio.Exceptions;

namespace Blog.Dominio.CommandHandlers;

public class InitArticleCommandHandler(IEventStore eventStore) : ICommandHandler<ArticleCommands.InitArticle>
{
    public void Handle(ArticleCommands.InitArticle command)
    {
        AssertIfTitleIsEmpty(command.Title);

        AssertIfLengthOfBlocksIsCorrect(command.Block);

        AssertIfLengthOfAuthorsIsCorrect(command.Authors);

        AssertTagLengthIsCorrect(command);

        eventStore.AppendEvent(command.Id,
            new ArticleEvents.ArticleInitiated(command.Id, command.Title, command.Block, command.Authors, command.Tags, command.CreatedAt));
    }

    private static void AssertTagLengthIsCorrect(ArticleCommands.InitArticle command)
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