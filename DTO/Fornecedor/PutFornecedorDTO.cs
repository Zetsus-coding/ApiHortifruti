using System.ComponentModel.DataAnnotations;

public class PutFornecedorDTO
{
    [Required(ErrorMessage = "O ID do fornecedor é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "O ID do fornecedor deve ser um número válido positivo")]
    public int Id { get; set; }


    [Required(ErrorMessage = "O nome fantasia do fornecedor é obrigatório")]
    [StringLength(100, ErrorMessage = "O nome fantasia do fornecedor não pode exceder 100 caracteres")]
    public string NomeFantasia { get; set; }


    [Required(ErrorMessage = "O cadastro de pessoa é obrigatório")]
    [StringLength(20, ErrorMessage = "O cadastro de pessoa não pode exceder 20 caracteres")]
    public string CadastroPessoa { get; set; }


    /// <summary>
    ///     Telefone no formato (XX) XXXXX-XXXX ou (XX) XXXX-XXXX
    /// </summary>
    /// <example> (11) 98765-4321 </example>
    [Required(ErrorMessage = "O telefone \"principal\" é obrigatório")]
    [RegularExpression(@"^(\(?\d{2}\)?\s?)?(\d{4,5}-?\d{4})$", ErrorMessage = "O formato para telefone \"principal\" é inválido.")] // "(DDD) ? (Valida número 4n||5n - 4n)
    public string Telefone { get; set; }


    [RegularExpression(@"^(\(?\d{2}\)?\s?)?(\d{4,5}-?\d{4})$", ErrorMessage = "O formato para telefone \"extra\" é inválido.")] // "(DDD) ? (Valida número 4n||5n - 4n)
    public string? TelefoneExtra { get; set; }


    [Required(ErrorMessage = "O email do fornecedor é obrigatório")]
    [EmailAddress(ErrorMessage = "O formato do email é inválido")]
    [StringLength(100, ErrorMessage = "O email não pode exceder 100 caracteres")]
    public string Email { get; set; }

    
    public bool Ativo { get; set; }
}