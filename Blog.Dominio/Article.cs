using Blog.Abstractions;
using Blog.Dominio.Events;

namespace Blog.Dominio;

public class Article : AggregateRoot
{
    public const string NO_PUEDE_TENER_MAS_DE_20_BLOQUES = "El articulo no puede contener más de 20 bloques";
    public const string DEBE_CONTENER_AL_MENOS_UN_BLOQUE = "No se puede crear un articulo sin bloques.";
    public const string EL_TITULO_NO_PUEDE_SER_VACIO = "No se puede crear un articulo con el titulo vacío.";
    public const string DEBE_CONTENER_AL_MENOS_UN_AUTOR = "El articulo debe contener al menos un autor.";

    public const string DEBE_CONTENER_AL_MENOS_UN_TAG_DESCRIPTIVO =
        "El articulo debe contener al menos un tag descriptivo.";

    public DateTime CreatedAt { get; private set; }

    public void Apply(ArticleEvents.ArticleInitiated @event)
    {
        Id = @event.Id;
        CreatedAt = @event.CreatedAt;
    }
}