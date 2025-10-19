using System.Text.Json.Serialization;

namespace Hortifruti.Domain;

public partial class Motivo_movimentacao
{
    public int Id { get; set; }

    public string TipoMovimentacao { get; set; } = null!;

    public bool Ativo { get; set; }

    [JsonIgnore]
    public virtual ICollection<Entrada> Entrada { get; set; } = new List<Entrada>();

    [JsonIgnore]
    public virtual ICollection<Saida> Saida { get; set; } = new List<Saida>();
}
