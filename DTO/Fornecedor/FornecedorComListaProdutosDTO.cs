public class FornecedorComListaProdutosDTO
{
    public int Id { get; set; }

    public string NomeFantasia { get; set; } = null!;

    public string CadastroPessoa { get; set; } = null!;

    public string Telefone { get; set; } = null!;

    public string? TelefoneExtra { get; set; }

    public string Email { get; set; } = null!;

    public IEnumerable<GetProdutoComDetalhesFornecimentoDTO> Produtos { get; set; } = new List<GetProdutoComDetalhesFornecimentoDTO>();
}