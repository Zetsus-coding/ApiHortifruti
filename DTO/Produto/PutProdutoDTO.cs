using System.ComponentModel.DataAnnotations;

public class PutProdutoDTO
{
    // O id é inserido pelo angular no corpo da requisição (e não pelo "usuário em si")
    [Required(ErrorMessage = "O ID do produto é obrigatório para atualizar o produto")]
    [Range(1, int.MaxValue, ErrorMessage = "Por favor, informe um ID de produto válido")]
    public int IdProduto { get; set; }


    [Required(ErrorMessage = "Por favor, informe uma categoria")]
    [Range(1, int.MaxValue, ErrorMessage = "Por favor, informe uma categoria válida")]
    public int CategoriaId { get; set; }


    [Required(ErrorMessage = "Por favor, informe uma unidade de medida")]
    [Range(1, int.MaxValue, ErrorMessage = "Por favor, informe uma unidade de medida válida")]
    public int UnidadeMedidaId { get; set; }


    [Required(ErrorMessage = "O nome do produto é obrigatório")]
    [StringLength(75, ErrorMessage = "O nome do produto não pode exceder 75 caracteres")]
    public string Nome { get; set; }


    // UNIQUE
    [Required(ErrorMessage = "O código do produto é obrigatório")]
    [StringLength(50, ErrorMessage = "A código do produto não pode exceder 50 caracteres")]
    public string Codigo { get; set; }
    

    [StringLength(150, ErrorMessage = "A descrição do produto não pode exceder 150 caracteres")]
    public string? Descricao { get; set; }


    [Required(ErrorMessage = "O preço do produto é obrigatório")]
    [ValidacaoCampoPreco]
    public decimal Preco { get; set; }


    [Required(ErrorMessage = "A quantidade mínima do produto é obrigatória")]
    [Range(typeof(decimal), "0", "99999999,99", ErrorMessage = "A quantidade mínima não pode ser negativa")]
    public decimal QuantidadeMinima { get; set; }


    // Tem validação?
    public bool Ativo { get; set; }
}