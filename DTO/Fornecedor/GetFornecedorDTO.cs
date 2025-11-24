public class GetFornecedorDTO
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string CadastroPessoa { get; set; }
    public string Telefone { get; set; }
    public string? TelefoneExtra { get; set; }
    public string Email { get; set; }
    public DateOnly DataRegistro { get; set; }
    public bool Ativo { get; set; }
}