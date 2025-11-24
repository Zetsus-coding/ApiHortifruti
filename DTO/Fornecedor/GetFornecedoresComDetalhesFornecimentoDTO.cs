public class GetFornecedoresComDetalhesFornecimentoDTO
{
    // Informações do Fornecedor
    public string NomeFantasia { get; set; } = null!;

    public string Telefone { get; set; } = null!;

    public string? TelefoneExtra { get; set; }

    public string Email { get; set; } = null!;

    public bool Ativo { get; set; }

    // Detalhes do Fornecimento
    public string CodigoFornecedor { get; set; }
    public bool Disponibilidade { get; set; }
}