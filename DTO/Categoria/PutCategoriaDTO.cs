using System.ComponentModel.DataAnnotations;
public class PutCategoriaDTO
{
    [Required(ErrorMessage = "O ID da categoria é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "O ID da categoria deve ser um número positivo")]
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome da categoria é obrigatório")]
    [StringLength(50, ErrorMessage = "O nome da categoria não pode exceder 50 caracteres")]
    public string Nome { get; set; }
}