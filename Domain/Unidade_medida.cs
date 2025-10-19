using System.Text.Json.Serialization;

namespace ApiHortifruti.Domain;

public partial class Unidade_medida
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Abreviacao { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}
