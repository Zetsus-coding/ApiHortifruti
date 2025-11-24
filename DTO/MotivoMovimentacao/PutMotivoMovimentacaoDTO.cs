using System.ComponentModel.DataAnnotations;

namespace ApiHortifruti.Domain; // Ajuste o namespace se necessário

public class PutMotivoMovimentacaoDTO
{
    [Required(ErrorMessage = "A descrição do motivo é obrigatória")]
    [StringLength(20, ErrorMessage = "O motivo não pode exceder 20 caracteres")]
    public string Motivo { get; set; }

    [Required(ErrorMessage = "O status (ativo/inativo) é obrigatório")]
    public bool Ativo { get; set; }
}