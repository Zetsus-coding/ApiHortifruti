namespace ApiHortifruti.DTO.Funcionario;

public class GetFuncionarioDTO
{
    public int Id { get; set; }
    public string Cpf { get; set; } = null!;
    public string Rg { get; set; } = null!;
    public string Nome { get; set; } = null!;
    public string Telefone { get; set; } = null!;
    public string? TelefoneExtra { get; set; }
    public string Email { get; set; } = null!;
    public string ContaBancaria { get; set; } = null!;
    public string AgenciaBancaria { get; set; } = null!;
    public bool Ativo { get; set; }
}
