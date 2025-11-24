namespace ApiHortifruti.DTO.Saida;

public class GetSaidaSimplesDTO
{
    public int Id { get; set; }
    public string Motivo { get; set; } = null!;
    public decimal ValorFinal { get; set; }
    public DateOnly DataSaida { get; set; }
    public TimeOnly HoraSaida { get; set; }
}
