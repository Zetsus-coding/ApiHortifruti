using System.ComponentModel.DataAnnotations;

public class GetEntradaSimplesDTO
{
    public int Id { get; set; } // Identificação única da entrada


    public string NomeFantasiaFornecedor { get; set; }


    public string Motivo { get; set; }


    public decimal PrecoTotal { get; set; }


    public DateOnly DataCompra { get; set; }
}