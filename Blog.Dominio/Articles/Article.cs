using Blog.Abstractions;

namespace Blog.Dominio.Articles;

public class Article : AggregateRoot
{
    public const string NO_PUEDE_TENER_MAS_DE_20_BLOQUES = "El articulo no puede contener más de 20 bloques";
    public const string DEBE_CONTENER_AL_MENOS_UN_BLOQUE = "No se puede crear un articulo sin bloques.";
    public const string EL_TITULO_NO_PUEDE_SER_VACIO = "No se puede crear un articulo con el titulo vacío.";
    public const string DEBE_CONTENER_AL_MENOS_UN_AUTOR = "El articulo debe contener al menos un autor.";

    public const string DEBE_CONTENER_AL_MENOS_UN_TAG_DESCRIPTIVO =
        "El articulo debe contener al menos un tag descriptivo.";

    public const string EL_ID_NO_PUEDE_SER_VACIO = "El Id del articulo no puede ser vacío.";

    public DateTime CreatedAt { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public IReadOnlyList<object> Block { get; private set; } = [];
    public IReadOnlyList<object> Authors { get; private set; } = [];
    public IReadOnlyList<object> Tags { get; private set; } = [];

    public void Apply(ArticleEvents.ArticleInitiated @event)
    {
        Id = @event.Id;
        Title = @event.Title;
        Block = @event.Block;
        Authors = @event.Authors;
        Tags = @event.Tags;
        CreatedAt = @event.CreatedAt;
    }
}