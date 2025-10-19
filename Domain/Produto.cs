using System.Text.Json.Serialization;

namespace ApiHortifruti.Domain;

public partial class Produto
{
    public int Id { get; set; }

    public int CategoriaId { get; set; }

    public int UnidadeMedidaId { get; set; }

    public string? Nome { get; set; }

    public string? Codigo { get; set; }
        
    public string? Descricao { get; set; }

    public decimal Preco { get; set; }

    public int QuantidadeAtual { get; set; }

    public int QuantidadeMinima { get; set; }

    /// <summary>
    /// Talvez alterar quant. máx para permitir NULL?
    /// </summary>
    public int QuantidadeMaxima { get; set; }

    public virtual Categoria? Categoria { get; set; }
 
    public virtual Unidade_medida? UnidadeMedida { get; set; }

    [JsonIgnore]
    public virtual ICollection<Fornecedor_produto> FornecedorProdutos { get; set; } = new List<Fornecedor_produto>();

    [JsonIgnore]
    public virtual ICollection<Historico_produto> HistoricoProdutos { get; set; } = new List<Historico_produto>();

    [JsonIgnore]
    public virtual ICollection<Item_entrada> ItemEntrada { get; set; } = new List<Item_entrada>();

    [JsonIgnore]
    public virtual ICollection<Item_saida> ItemSaida { get; set; } = new List<Item_saida>();
}
