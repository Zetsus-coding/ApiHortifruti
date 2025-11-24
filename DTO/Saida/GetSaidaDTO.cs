public class GetSaidaDTO
{
    public int Id { get; set; }
    public int MotivoMovimentacaoId { get; set; }
    public string Motivo { get; set; } = null!;
    public string? CadastroCliente { get; set; }
    public decimal ValorTotal { get; set; }
    public bool Desconto { get; set; }
    public decimal? ValorDesconto { get; set; }
    public decimal ValorFinal { get; set; }
    public DateOnly DataSaida { get; set; }
    public TimeOnly HoraSaida { get; set; }
    public ICollection<ItemSaidaDTO> Itens { get; set; } = new List<ItemSaidaDTO>();
}
