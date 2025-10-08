namespace Hortifruti.DTO;

public class ProdutoDTO
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public int UnidadeMedidaId { get; set; } // Necessário?
    public string UnidadeMedidaSigla { get; set; } = null!;
    public int QuantidadeAtual { get; set; }
    public int QuantidadeMinima { get; set; }
    public string Codigo { get; set; } = null!;
    public decimal Preco { get; set; }
    public int CategoriaId { get; set; } // Necessário?
    public string CategoriaNome { get; set; } = null!;
}