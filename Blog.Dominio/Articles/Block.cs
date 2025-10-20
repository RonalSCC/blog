namespace Blog.Dominio.Articles;

public class Block
{
    public const string NOT_ADD_BLOCK_WITHOUT_CONTENT = "No se puede agregar un bloque sin contenido.";
    public const string MUST_EXISTS_AN_ARTICLE_TO_ADD_BLOCK = "Debe existir un artículo para agregar un bloque.";

    public enum BlockType
    {
        Text = 1
    }
}