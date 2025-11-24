using System.ComponentModel.DataAnnotations;

public class PutFuncionarioDTO
{
    [Required(ErrorMessage = "O ID é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "O ID deve ser um número válido e positivo")]
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(100, ErrorMessage = "O nome não pode exceder 100 caracteres")]
    public string Nome { get; set; }

    [Required(ErrorMessage = "O telefone principal é obrigatório")]
    [StringLength(20, ErrorMessage = "O telefone não pode exceder 20 caracteres")]
    [RegularExpression(@"^(\(?\d{2}\)?\s?)?(\d{4,5}-?\d{4})$", ErrorMessage = "Formato de telefone inválido")]
    public string Telefone { get; set; }

    [StringLength(20, ErrorMessage = "O telefone extra não pode exceder 20 caracteres")]
    public string? TelefoneExtra { get; set; }

    [Required(ErrorMessage = "O e-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "Formato de e-mail inválido")]
    [StringLength(100, ErrorMessage = "O e-mail não pode exceder 100 caracteres")]
    public string Email { get; set; }

    [Required(ErrorMessage = "A conta bancária é obrigatória")]
    [StringLength(20, ErrorMessage = "A conta bancária não pode exceder 20 caracteres")]
    public string ContaBancaria { get; set; }

    [Required(ErrorMessage = "A agência bancária é obrigatória")]
    [StringLength(20, ErrorMessage = "A agência bancária não pode exceder 20 caracteres")]
    public string AgenciaBancaria { get; set; }
    
    public bool Ativo { get; set; }
}