using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
public class PutMotivoMovimentacaoDTO
{
    [JsonIgnore]
    public int Id { get; set; }

    [Required(ErrorMessage = "A descrição do motivo é obrigatória")]
    [StringLength(20, ErrorMessage = "O motivo não pode exceder 20 caracteres")]
    public string Motivo { get; set; }

    [Required(ErrorMessage = "O status (ativo/inativo) é obrigatório")]
    public bool Ativo { get; set; }
}