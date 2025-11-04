using System.ComponentModel.DataAnnotations;
using ApiHortifruti.Domain.DTO.CustomAnnotation;

namespace ApiHortifruti.Domain.DTO.ItemEntrada;

public class ItemEntradaDTO
{
    [Required(ErrorMessage = "O ID do produto é obrigatório no item da entrada")]
    public int ProdutoId { get; set; }


    [Required(ErrorMessage = "A quantidade não pode ser nula/vazia")]
    [Range(0, 99999999.99, MinimumIsExclusive = true, ErrorMessage = "A quantidade deve ser maior que zero")]
    public decimal Quantidade { get; set; }

    
    public string? Lote { get; set; }


    [DataNaoPassada(ErrorMessage = "Produto com validade inválida (já vencida)")]
    public string? Validade { get; set; }


    [Required(ErrorMessage = "O preço do produto é obrigatório")] // Deve ser obrigatório?
    //[RegularExpression(@"^(0|\d{0,16}(\.\d{0,2})?)$", ErrorMessage = "Formato de preço inválido")] // Formato válido
    [Range(0, 99999999.99, MinimumIsExclusive = true, ErrorMessage = "O preço deve ser maior que zero")] // Valor maior que zero
    public decimal PrecoUnitario { get; set; }
}
