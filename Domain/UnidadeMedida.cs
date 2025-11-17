using System.Text.Json.Serialization;

namespace ApiHortifruti.Domain;

public class UnidadeMedida
{
    public int Id { get; private set; }

    public string Nome { get; private set; } = null!;

    public string Abreviacao { get; private set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Produto> Produto { get; private set; } = new List<Produto>();

    //public UnidadeMedida() { }

    // public UnidadeMedida(string nome, string abreviacao)
    // {
    //     if (string.IsNullOrWhiteSpace(nome))
    //         throw new ArgumentException("O nome da unidade de medida não pode ser vazio.", nameof(nome));

    //     if (string.IsNullOrWhiteSpace(abreviacao))
    //         throw new ArgumentException("A abreviação da unidade de medida não pode ser vazia.", nameof(abreviacao));

    //     if (abreviacao.Length > 10)
    //         throw new ArgumentException("A abreviação da unidade de medida não pode exceder 10 caracteres.", nameof(abreviacao));

    //     Nome = nome;
    //     Abreviacao = abreviacao;
    // }

    // Métodos de manipulação do objeto
}
