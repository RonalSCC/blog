namespace Blog.Dominio.Articles;

public class Block
{
    public const string NOT_ADD_BLOCK_WITHOUT_CONTENT = "No se puede agregar un bloque sin contenido.";
    public const string MUST_EXISTS_AN_ARTICLE_TO_ADD_BLOCK = "Debe existir un artículo para agregar un bloque.";
    public const string THE_CONTENT_OF_A_TEXT_BLOCK_CANNOT_EXCEED_2000_CHARACTERS = "El contenido del bloque en el tipo texto no puede superar los 2000 caracteres.";

    public enum BlockType
    {
        Text = 1
    }
}