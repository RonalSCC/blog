using Blog.Abstractions.EventSourcing;
using Blog.Dominio.Articles.Exceptions;

namespace Blog.Dominio.Articles.CommandHandlers;

public class AddBlockCommandHandler(IEventStore eventStore) : ICommandHandlerAsync<ArticleCommands.AddBlock>
{
    public async Task HandleAsync(ArticleCommands.AddBlock command, CancellationToken ct)
    {
        await GetArticleOrExceptionIfNotExists(command, ct);
        throw new AddBlockException(Block.NOT_ADD_BLOCK_WITHOUT_CONTENT);
    }

    private async Task<Articles.Article> GetArticleOrExceptionIfNotExists(ArticleCommands.AddBlock command, CancellationToken ct)
    {
        var article = await eventStore.GetAggregateRootAsync<Articles.Article>(command.Id, ct);
        if(article is null)
            throw new AddBlockException(Block.MUST_EXISTS_AN_ARTICLE_TO_ADD_BLOCK);

        return article;
    }
}