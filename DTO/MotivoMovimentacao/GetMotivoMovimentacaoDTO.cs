namespace ApiHortifruti.DTO.MotivoMovimentacao;

public class GetMotivoMovimentacaoDTO
{
    public int Id { get; set; }
    public string Motivo { get; set; } = null!;
    public bool Ativo { get; set; }
}
