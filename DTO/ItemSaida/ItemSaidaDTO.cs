using System.ComponentModel.DataAnnotations;
using ApiHortifruti.DTO.CustomAnnotation;

namespace ApiHortifruti.DTO.ItemSaida;

public class ItemSaidaDTO
{
    [Required(ErrorMessage = "O ID do produto é obrigatório no item da saída")]
    public int ProdutoId { get; set; }


    [Required(ErrorMessage = "A quantidade não pode ser nula/vazia")]
    [Range(0, 99999999.99, MinimumIsExclusive = true, ErrorMessage = "A quantidade deve ser maior que zero")]
    public int Quantidade { get; set; }


    //public decimal Valor { get; set; } // É realmente necessário?
}
