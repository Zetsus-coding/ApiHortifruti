using System.Text.Json.Serialization;

namespace ApiHortifruti.Domain;

public partial class Cargo
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string? Descricao { get; set; }

    public bool Ativo { get; set; }

    [JsonIgnore]
    public virtual ICollection<Funcionario> Funcionarios { get; set; } = new List<Funcionario>();
    
    [JsonIgnore]
    public virtual ICollection<Permissao> Permissaos { get; set; } = new List<Permissao>();
}
