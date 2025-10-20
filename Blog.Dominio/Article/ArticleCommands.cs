namespace Blog.Dominio.Article;

public abstract record ArticleCommands
{
    private ArticleCommands() { }
    public record InitArticle(
        string Id,
        string Title,
        List<object> Block,
        List<object> Authors,
        List<object> Tags,
        DateTime CreatedAt) : ArticleCommands;
}