public class GetEntradaDTO
{
    // public int Id { get; set; }
    public string NumeroNota { get; set; }
    public string NomeFantasiaFornecedor { get; set; }
    public int MotivoMovimentacaoId { get; set; }
    public string Motivo { get; set; }
    public decimal PrecoTotal { get; set; }
    public DateOnly DataCompra { get; set; }
    public IEnumerable<ItemEntradaDTO> Itens { get; set; } = new List<ItemEntradaDTO>();
}
