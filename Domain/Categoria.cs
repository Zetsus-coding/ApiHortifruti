using System.Text.Json.Serialization;

namespace ApiHortifruti.Domain;

public partial class Categoria
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    [JsonIgnore] // Evitar referência circular
    public virtual ICollection<Produto> Produto { get; set; } = new List<Produto>();

    public Categoria() {}

    // DEPOIS EU TERMINO (TODO)




    // ACHO Q OS TESTES SÃO MAIS IMPORTANTES DO QUE ESSAS EXCEPTIONS!

    // HOUVE UM PEQUENO PROBLEMA, PARA EU RODAR MEUS TESTES A API PRECISA ESTAR RODANDO DIREITINHO, ENTÃO EU VOU DEIXAR ESSA PARTE PARA DEPOIS QUANDO VC VOLTAR.    

    // public Categoria(string nome)
    // {
    //     if (string.IsNullOrWhiteSpace(nome))
    //         throw new ArgumentException("O nome da categoria não pode ser vazio ou nulo.", nameof(nome));

    //     if (nome.Length > 50)
    //         throw new ArgumentException("O nome da categoria não pode exceder 50 caracteres.", nameof(nome));
    //     Nome = nome;
    // }
}