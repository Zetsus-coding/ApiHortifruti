using System.ComponentModel.DataAnnotations;

public class ItemEntradaDTO
{
    [Required(ErrorMessage = "O ID do produto é obrigatório no item da entrada")]
    public int ProdutoId { get; set; }


    [Required(ErrorMessage = "A quantidade não pode ser nula/vazia")]
    [Range(0, 99999999.99, MinimumIsExclusive = true, ErrorMessage = "A quantidade deve ser maior que zero")]
    public decimal Quantidade { get; set; }

    
    public string? Lote { get; set; }


    [DataNaoPassada(ErrorMessage = "Produto com validade inválida (produto vencido)")]
    public string? Validade { get; set; }


    [Required(ErrorMessage = "O preço do produto é obrigatório")] // Deve ser obrigatório?
    [ValidacaoCampoPreco()] // Preço deve ser maior ou igual a zero e com até duas casas decimais
    public decimal PrecoUnitario { get; set; }

    // Campo de código do fornecedor (opcional), para ser usado durante a criação de entradas caso o registro em fornecedor produto não exista
    [Required(ErrorMessage = "O código do fornecedor é obrigatório no item da entrada")]
    public string CodigoFornecedor { get; set; }
}
