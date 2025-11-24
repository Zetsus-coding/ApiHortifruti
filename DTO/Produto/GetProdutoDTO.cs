public class GetProdutoDTO
{
    public int Id { get; set; }
    public string Nome { get; set;}
    public string Codigo { get; set; }
    public decimal Preco { get; set; }
    public string Descricao { get; set; }
    public decimal QuantidadeAtual { get; set; }
    public decimal QuantidadeMinima { get; set; }
    public bool Ativo { get; set; }

    // Relacionamentos
    public GetCategoriaDTO? Categoria { get; set; }
    public GetUnidadeMedidaDTO? UnidadeMedida { get; set; }
}