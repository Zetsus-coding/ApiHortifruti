namespace ApiHortifruti.Domain;

public partial class HistoricoProduto
{
    public int Id { get; set; }

    public int ProdutoId { get; set; }

    public decimal PrecoProduto { get; set; }

    public DateOnly DataAlteracao { get; set; }

    // public int FuncionarioId { get; set; }

    public virtual Produto Produto { get; set; } = null!;
}
