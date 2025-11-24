using ApiHortifruti.DTO.ItemEntrada;

namespace ApiHortifruti.DTO.Entrada;

public class GetEntradaDTO
{
    public int Id { get; set; }
    public int FornecedorId { get; set; }
    public string NomeFantasiaFornecedor { get; set; } = null!;
    public int MotivoMovimentacaoId { get; set; }
    public string Motivo { get; set; } = null!;
    public decimal PrecoTotal { get; set; }
    public DateOnly DataCompra { get; set; }
    public string NumeroNota { get; set; } = null!;
    public ICollection<ItemEntradaDTO> Itens { get; set; } = new List<ItemEntradaDTO>();
}
