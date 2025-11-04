using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiHortifruti.Domain;

public partial class UsuarioDTO
{
    [Required(ErrorMessage = "O login é obrigatório")]
    [StringLength(30, ErrorMessage = "O login não pode exceder 30 caracteres")]
    public string Login { get; set; } = null!;

    [Required(ErrorMessage = "A senha é obrigatória")]
    [StringLength(100, ErrorMessage = "A senha não pode exceder 100 caracteres")]
    public string Senha { get; set; } = null!;

    [Required(ErrorMessage = "É necessário informar se o usuário está ativo")]
    public bool Ativo { get; set; }

}
