using System.ComponentModel.DataAnnotations;

public class PutFornecedorProdutoDTO
{
    [Required]
    public int FornecedorId { get; set; }

    [Required]
    public int ProdutoId { get; set; }

    [Required]
    public string CodigoFornecedor { get; set; } = null!;

    public bool Disponibilidade { get; set; }
}
