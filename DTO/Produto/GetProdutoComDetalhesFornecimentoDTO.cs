public class GetProdutoComDetalhesFornecimentoDTO
{
    // Detalhes do relacionamento de FornecedorProduto
    public string CodigoFornecedor { get; set; }
    public DateOnly DataRegistro { get; set; }
    public bool Disponibilidade { get; set; }

    // Detalhes de Produto
    public int ProdutoId { get; set; }
    public string NomeProduto { get; set; }
    public string DescricaoProduto { get; set; }
    public string Codigo { get; set; }
    public decimal PrecoAtual { get; set; }

    // Detalhes de categoria e unidade de medida
    public GetCategoriaDTO CategoriaDTO { get; set; }
    public GetUnidadeMedidaDTO UnidadeMedidaDTO { get; set; }
}