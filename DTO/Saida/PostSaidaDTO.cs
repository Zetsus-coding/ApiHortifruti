using ApiHortifruti.Domain.DTO.CustomAnnotation;
using ApiHortifruti.Domain.DTO.ItemSaida;
using System.ComponentModel.DataAnnotations;

public partial class PostSaidaDTO
{
    [Required]
    [Range(1, int.MaxValue)]
    public int MotivoMovimentacaoId { get; set; }


    // [Required]
    // [Range(1, int.MaxValue)]
    // public int FuncionarioId { get; set; }


    public string? CadastroCliente { get; set; } // OPCIONAL: CPF ou CNPJ, caso informado


    [Required]
    [ValidacaoCampoPreco]
    public decimal ValorTotal { get; set; } // Valor total antes de qualquer desconto

    [Required]
    public bool Desconto { get; set; } // Indica se houve desconto na saída


    [RequiredIfTrue("Desconto")] // Só é obrigatório se o campo Desconto for true
    public decimal? ValorDesconto { get; set; }


    // [Required]
    // [ValidacaoCampoPreco]
    // public decimal ValorFinal { get; set; } // Se Desconto for true, ValorTotal - ValorDesconto, senão igual a ValorTotal


    // [DataNaoFutura]
    // public DateOnly DataSaida { get; set; } // Basicamente var = DateOnly.FromDateTime(DateTime.Now);

    
    // public TimeOnly HoraSaida { get; set; } // Basicamente var = TimeOnly.FromDateTime(DateTime.Now);


    [Required]
    [MinLength(1, ErrorMessage = "É obrigatório informar ao menos um item na entrada")]
    public List<ItemSaidaDTO> ItemSaida { get; set; } = new List<ItemSaidaDTO>();
}