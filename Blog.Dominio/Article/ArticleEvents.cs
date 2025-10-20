namespace Blog.Dominio.Article;

public abstract record ArticleEvents
{
    private ArticleEvents(){}
    
    public record ArticleInitiated(
        string Id,
        string Title,
        List<object> Block,
        List<object> Authors,
        List<object> Tags,
        DateTime CreatedAt) : ArticleEvents;
    
}