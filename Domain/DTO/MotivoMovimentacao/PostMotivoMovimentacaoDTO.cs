using System.ComponentModel.DataAnnotations;

public class PostMotivoMovimentacaoDTO
{
    [Required(ErrorMessage = "O campo 'Descrição' é obrigatório.")]
    public string Motivo { get; set; }
}