using System.Text.Json.Serialization;

namespace Hortifruti.Domain;

public partial class Saida
{
    public int Id { get; set; }

    public int MotivoMovimentacaoId { get; set; }

    public int FuncionarioId { get; set; }

    public string? CadastroCliente { get; set; }

    public decimal ValorTotal { get; set; }

    public bool Desconto { get; set; }

    public decimal? ValorDesconto { get; set; }

    public decimal ValorFinal { get; set; }

    public DateOnly DataSaida { get; set; }

    public TimeOnly HoraSaida { get; set; }

    public virtual Funcionario Funcionario { get; set; } = null!;

    public virtual Motivo_movimentacao MotivoMovimentacao { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Item_saida> ItemSaida { get; set; } = new List<Item_saida>();
}
