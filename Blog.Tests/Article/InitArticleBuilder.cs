using Blog.Dominio.Comandos;

namespace Blog.Tests.Article;

public class InitArticleBuilder
{
    private ArticleCommands.InitArticle _command = new(
        Guid.CreateVersion7().ToString(),
        "My first article",
        [new object()],
        [new object()],
        [new object()],
        DateTime.UtcNow);

    public InitArticleBuilder WithId(string id)
    {
        _command = _command with { Id = id };
        return this;
    }
    
    public InitArticleBuilder WithTitle(string title)
    {
        _command = _command with { Title = title };
        return this;
    }
    
    public InitArticleBuilder WithBlocks(List<object> block)
    {
        _command = _command with { Block = block };
        return this;
    }
    
    public InitArticleBuilder WithAuthors(List<object> authors)
    {
        _command = _command with { Authors = authors };
        return this;
    }
    
    public InitArticleBuilder WithTags(List<object> tags)
    {
        _command = _command with { Tags = tags };
        return this;
    }
    
    public InitArticleBuilder WithCreatedAt(DateTime createdAt)
    {
        _command = _command with { CreatedAt = createdAt };
        return this;
    }
    
    public ArticleCommands.InitArticle Build() => _command;
    
}