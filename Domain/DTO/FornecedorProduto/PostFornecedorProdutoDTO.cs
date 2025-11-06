
using System.ComponentModel.DataAnnotations;

public class PostFornecedorProdutoDTO
{
    [Required(ErrorMessage = "É obrigatório informar o fornecedor.")]
    [Range(1, int.MaxValue, ErrorMessage = "Informe um fornecedor válido.")]
    public int FornecedorId { get; set; }

    [Required(ErrorMessage = "É obrigatório informar o produto.")]
    [Range(1, int.MaxValue, ErrorMessage = "Informe um produto válido.")]
    public int ProdutoId { get; set; }

    [Required(ErrorMessage = "É obrigatório informar o código do fornecedor deste produto.")]
    [MaxLength(50, ErrorMessage = "O código do fornecedor deve ter no máximo 50 caracteres.")]
    public string CodigoFornecedor { get; set; } = null!;
 
}