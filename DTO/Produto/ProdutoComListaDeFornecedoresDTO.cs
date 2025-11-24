public class ProdutoComListaDeFornecedoresDTO
{
    public string Nome { get; set; }

    public string Codigo { get; set; }

    public decimal Preco { get; set; }

    public decimal QuantidadeAtual { get; set; }

    public decimal QuantidadeMinima { get; set; }

    public virtual GetCategoriaDTO? Categoria { get; set; }
 
    public virtual GetUnidadeMedidaDTO? UnidadeMedida { get; set; }

    public IEnumerable<GetFornecedoresComDetalhesFornecimentoDTO> Fornecedores { get; set; } = new List<GetFornecedoresComDetalhesFornecimentoDTO>();
}