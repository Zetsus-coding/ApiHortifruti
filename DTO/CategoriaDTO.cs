namespace Hortifruti.DTO;

public class CategoriaDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;

    public List<ProdutoDTO>? Produtos { get; set; }
}