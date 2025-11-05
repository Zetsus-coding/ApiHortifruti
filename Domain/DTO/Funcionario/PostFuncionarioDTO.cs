using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiHortifruti.Domain;

public partial class PostFuncionarioDTO
{
    [Required(ErrorMessage = "Informe o cargo do funcionário")]
    public int CargoId { get; set; }

    [Required(ErrorMessage = "Informe o CPF do funcionário")]
    [StringLength(14, MinimumLength = 11, ErrorMessage = "O CPF deve ter 11 ou 14 caracteres (com pontuação).")] 
    [CpfValidation(ErrorMessage = "O CPF informado é inválido. Verifique os números.")]
    public string Cpf { get; set; } = null!;

    [Required(ErrorMessage = "Informe o rg do funcionário")]
    [StringLength(20, ErrorMessage = "O RG não pode exceder 20 caracteres.")]
    [RegularExpression(@"^(\d{1,2}\.?\d{3}\.?\d{3}-?[\dX]|[0-9]{9})$", ErrorMessage = "O formato do RG é inválido. Use DD.DDD.DDD-X ou apenas números.")]
    public string Rg { get; set; } = null!;

    [Required(ErrorMessage = "Informe o nome do funcionário")]
    [StringLength(100, ErrorMessage = "O nome do funcionário não pode exceder 100 caracteres")]
    public string Nome { get; set; } = null!;

    [Required(ErrorMessage = "O telefone \"principal\" é obrigatório")]
    [RegularExpression(@"^(\(?\d{2}\)?\s?)?(\d{4,5}-?\d{4})$", ErrorMessage = "O formato para telefone \"principal\" é inválido.")]
    public string Telefone { get; set; } = null!;
    
    [RegularExpression(@"^(\(?\d{2}\)?\s?)?(\d{4,5}-?\d{4})$", ErrorMessage = "O formato para telefone \"Extra\" é inválido.")]
    public string? TelefoneExtra { get; set; }

    [Required(ErrorMessage = "Informe o email do funcionário")]
    [EmailAddress(ErrorMessage = "O formato do email é inválido")]
    [StringLength(100, ErrorMessage = "O email não pode exceder 100 caracteres")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Informe a conta bancaria do funcionário")]
    [StringLength(20, ErrorMessage = "A conta bancária não pode exceder 20 caracteres.")]
    public string ContaBancaria { get; set; } = null!;

    [Required(ErrorMessage = "Informe a agencia bancaria do funcionário")]
    [StringLength(4, ErrorMessage = "A agência bancária deve ter 4 caracteres.")]
    public string AgenciaBancaria { get; set; } = null!;

    [Required(ErrorMessage = "É necessário informar se o usuário está ativo")]
    public bool Ativo { get; set; }
    

}
