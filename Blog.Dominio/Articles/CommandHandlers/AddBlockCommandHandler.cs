using Blog.Abstractions.EventSourcing;
using Blog.Dominio.Articles.Exceptions;

namespace Blog.Dominio.Articles.CommandHandlers;

public class AddBlockCommandHandler(IEventStore eventStore) : ICommandHandlerAsync<ArticleCommands.AddBlock>
{
    public async Task HandleAsync(ArticleCommands.AddBlock command, CancellationToken ct)
    {
        CheckForEmptyBlockContentOrLengthExceed(command.Contenido);
        await GetArticleOrExceptionIfNotExists(command, ct);
        
        eventStore.AppendEvent(command.Id, new ArticleEvents.BlockAdded(command.Id, command.Contenido, command.Type));
    }

    private static void CheckForEmptyBlockContentOrLengthExceed(string contenido)
    {
        if(string.IsNullOrEmpty(contenido))
            throw new AddBlockException(Block.NOT_ADD_BLOCK_WITHOUT_CONTENT);
        
        if(contenido.Length > 2000)
            throw new AddBlockException(Block.THE_CONTENT_OF_A_TEXT_BLOCK_CANNOT_EXCEED_2000_CHARACTERS);
    }

    private async Task<Articles.Article> GetArticleOrExceptionIfNotExists(ArticleCommands.AddBlock command, CancellationToken ct)
    {
        var article = await eventStore.GetAggregateRootAsync<Articles.Article>(command.Id, ct);
        if(article is null)
            throw new AddBlockException(Block.MUST_EXISTS_AN_ARTICLE_TO_ADD_BLOCK);

        return article;
    }
}